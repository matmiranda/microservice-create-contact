using FiapGrupo57Fase2.DTO.Request;
using FiapGrupo57Fase2.Infrastructure.Exception;
using FiapGrupo57Fase2.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FiapGrupo57Fase2.Test.Helper
{
    [TestFixture]
    public class ValidatorHelperTests
    {
        [Test]
        public void Validar_ContatoInvalido_ThrowsCustomException()
        {
            // Arrange
            var contatoRequest = new ContatosPostRequest
            {
                Nome = "João",
                Telefone = "123",
                Email = "joao.silva@",
                DDD = 11,
                Regiao = "Sudeste"
            };

            // Act & Assert
            var ex = Assert.Throws<CustomException>(() => ValidatorHelper.Validar(contatoRequest));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(ex.Message, Is.EqualTo("O nome deve conter nome e sobrenome, separados por um espaço."));
            });
        }

        [Test]
        public void Validar_ContatoValido_DoesNotThrowException()
        {
            // Arrange
            var contatoRequest = new ContatosPostRequest
            {
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 11,
                Regiao = "Sudeste"
            };

            // Act & Assert
            Assert.DoesNotThrow(() => ValidatorHelper.Validar(contatoRequest));
        }
    }
}
