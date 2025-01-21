# FiapGrupo57Fase2

## Apresentação

Link do vídeo: https://www.youtube.com/watch?v=-kXW7Ow-Sr4

## Descrição
Este projeto é uma aplicação WebAPI desenvolvida pelo grupo 57 da FIAP. Nosso objetivo é implementar uma arquitetura robusta e escalável, seguindo as melhores práticas de desenvolvimento de software.

Além disso, este projeto foi criado para demonstrar o uso de Prometheus e Grafana para o monitoramento de uma API desenvolvida em .NET 8. Incluímos um aplicativo de console que realiza operações de criação, atualização, consulta e exclusão de contatos na API, permitindo uma análise detalhada do desempenho e da integridade do sistema.

## Estrutura do Projeto
O projeto está organizado em seis camadas para garantir o isolamento da lógica de domínio, desacoplamento e flexibilidade de adaptação. As principais camadas incluem:

- **Domain**: Inclui as entidades e interfaces de domínio.
- **DTO**: Contém os objetos de transferência de dados utilizados para comunicação entre as camadas.
- **Infrastructure**: Implementa os repositórios e outras dependências externas.
- **Repository**: Implementa os padrões de repositório para acesso a dados.
- **Service**: Contém a lógica de negócios e serviços da aplicação.
- **WebAPI**: Contém os controladores e configurações da WebAPI.

## Tecnologias Utilizadas
- .NET Core 8
- Swagger
- Dapper
- Mysql
- Test Nunit

## Como instalar prometheus e grafana

### Prometheus
1. Baixar prometheus https://github.com/prometheus/prometheus/releases/download/v3.0.1/prometheus-3.0.1.windows-amd64.zip
2. Extrair na pasta C:\Prometheus\prometheus-3.0.1.windows-amd64
3. Executar o comando PS C:\Prometheus\prometheus-3.0.1.windows-amd64> .\prometheus.exe
4. Abrir o navegador e executar http://localhost:9090/

### Grafana
1. Baixar grafana https://dl.grafana.com/enterprise/release/grafana-enterprise-11.4.0.windows-amd64.zip
2. Extrair na pasta C:\Grafana\grafana-v11.4.0
3. Executar o comando PS C:\Grafana\grafana-v11.4.0\bin> .\grafana-server.exe
4. Abrir o navegador e executar http://localhost:3000/login

## Testes do monitoramento com Prometheus e Grafana

### Console Teste - Program.cs

```C#
using Bogus;
using ConsoleTeste;
using System.Text;
using System.Text.Json;

class Program
{
    private static readonly List<int> PostContatos = new List<int>();
    private static readonly List<ContatosGetResponse> Getcontatos = new List<ContatosGetResponse>();
    private static readonly string url = "http://localhost:7040/Contatos";

    static async Task Main(string[] args)
    {
        var client = new HttpClient();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        //Cria contatos
        for (int i = 0; i < 50; i++)
        {
            var contato = GerarContatoAleatorio();
            var json = JsonSerializer.Serialize(contato);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var decodedResponseBody = System.Text.RegularExpressions.Regex.Unescape(responseBody);
            Console.WriteLine($"Response POST Contato {i + 1}: {(int)response.StatusCode} {response.ReasonPhrase} {decodedResponseBody}");

            if (response.IsSuccessStatusCode)
            {
                var contatoResponse = JsonSerializer.Deserialize<ContatosPostResponse>(responseBody, options);
                PostContatos.Add(contatoResponse.Id);
            }
        }

        // Obtém contatos pelo ID
        foreach (var id in PostContatos)
        {
            var response = await client.GetAsync($"{url}/{id}");
            var responseBodyBytes = await response.Content.ReadAsByteArrayAsync();
            var responseBody = Encoding.UTF8.GetString(responseBodyBytes);
            Console.WriteLine($"Response GET ID {id}: {(int)response.StatusCode} {response.ReasonPhrase} {responseBody}");

            if (response.IsSuccessStatusCode)
            {
                var contatoResponse = JsonSerializer.Deserialize<ContatosGetResponse>(responseBody, options);
                Getcontatos.Add(contatoResponse);
            }
        }

        // Obtém contatos pelo DDD
        foreach (var contato in Getcontatos)
        {
            var response = await client.GetAsync($"{url}/?ddd={contato.DDD}");
            var responseBodyBytes = await response.Content.ReadAsByteArrayAsync();
            var responseBody = Encoding.UTF8.GetString(responseBodyBytes);
            Console.WriteLine($"Response GET DDD {contato.DDD}: {(int)response.StatusCode} {response.ReasonPhrase} {responseBody}");
        }

        //Atualiza contatos
        foreach (var contato in Getcontatos)
        {
            var contatoAtualizado = new ContatosPutRequest
            {
                Id = contato.Id,
                Nome = contato.Nome + " Atualizado",
                Telefone = contato.Telefone,
                Email = contato.Email,
                DDD = contato.DDD,
                Regiao = contato.Regiao
            };

            var json = JsonSerializer.Serialize(contatoAtualizado);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);
            var responseBodyBytes = await response.Content.ReadAsByteArrayAsync();
            var responseBody = Encoding.UTF8.GetString(responseBodyBytes);
            Console.WriteLine($"Response PUT ID {contato.Id}: {(int)response.StatusCode} {response.ReasonPhrase} {responseBody}");
        }

        // Exclui contatos
        Console.WriteLine("Aguardando 30 segundos...");
        await Task.Delay(30000); // 30000 milissegundos = 30 segundos
        Console.WriteLine("30 segundos se passaram.");
        foreach (var contato in Getcontatos)
        {
            var response = await client.DeleteAsync($"{url}/{contato.Id}");
            var responseBodyBytes = await response.Content.ReadAsByteArrayAsync();
            var responseBody = Encoding.UTF8.GetString(responseBodyBytes);
            Console.WriteLine($"Response DELETE ID {contato.Id}: {(int)response.StatusCode} {response.ReasonPhrase} {responseBody}");
        }

    }

    static ContatosPostRequest GerarContatoAleatorio()
    {
        var faker = new Faker<ContatosPostRequest>()
            .RuleFor(c => c.Nome, f => f.Name.FullName())
            .RuleFor(c => c.Telefone, f => f.Phone.PhoneNumber("#########"))
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.DDD, f => f.Random.Int(1, 99));

        return faker.Generate();
    }
}

```

## Explicação do Código

### Criação de Contatos
Utilizamos a biblioteca Bogus para gerar dados fictícios de contatos de forma variada e realista. Isso nos permite simular diferentes cenários de uso da API.
O código gera 50 contatos aleatórios e os adiciona na API. Cada contato é serializado em JSON e enviado em uma requisição POST. As respostas são verificadas e os IDs dos contatos adicionados são armazenados.

### Atualização de Contatos
Após adicionar os contatos, o código faz requisições PUT para atualizar cada um dos contatos adicionados, adicionando "Atualizado" ao nome de cada contato.

### Consulta de Contatos
O código faz requisições GET para consultar contatos pelo DDD, utilizando os DDDs dos contatos armazenados na lista.

### Exclusão de Contatos
Após consultar os contatos, o código faz requisições DELETE para excluir cada um dos contatos adicionados, utilizando os IDs armazenados anteriormente.

## Monitoramento com Prometheus e Grafana
Este projeto foi criado para demonstrar como o Prometheus e o Grafana podem ser usados para monitorar uma API. O Prometheus coleta métricas da API, enquanto o Grafana é usado para visualizar essas métricas em dashboards. Isso é útil para garantir que a API está funcionando corretamente e para identificar possíveis problemas de desempenho.

## GitHub Actions Workflow
Este projeto utiliza GitHub Actions para automatizar o processo de integração contínua (CI) e configuração de monitoramento com Prometheus e Grafana. O workflow está definido no arquivo .github/workflows/dotnet.yml.

### Explicação do Workflow
- Build: Este job é executado em cada push ou pull request para a branch main. Ele configura o .NET, restaura as dependências, compila o projeto e publica os artefatos de build.
- Test: Este job depende do job de build. Ele baixa os artefatos de build, executa todos os testes e publica os resultados dos testes.
- Setup Monitoring: Configura o Prometheus e o Grafana usando contêineres Docker, verifica se ambos estão funcionando corretamente e exibe logs de erro do Grafana em caso de falha.

## Conclusão
Este projeto demonstra como criar, atualizar, consultar e excluir contatos em uma API desenvolvida em .NET 8, além de mostrar como monitorar a API usando Prometheus e Grafana. Isso é útil para garantir que a API está funcionando corretamente e para identificar possíveis problemas de desempenho.

Além disso, utilizamos o GitHub Actions para automatizar o processo de integração contínua (CI) e configuração de monitoramento. O workflow do GitHub Actions inclui etapas para compilar o projeto, executar testes e configurar Prometheus e Grafana em contêineres Docker. Isso garante que o código seja continuamente integrado e testado, e que o monitoramento esteja sempre configurado e funcionando corretamente.

Com essa abordagem, conseguimos manter a qualidade do código, detectar problemas rapidamente e garantir que a API esteja sempre monitorada e funcionando de forma eficiente.
