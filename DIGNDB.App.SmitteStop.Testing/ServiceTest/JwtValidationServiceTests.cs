using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.DependencyInjection;
using DIGNDB.App.SmitteStop.Core.Exceptions;
using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.DAL.DependencyInjection;
using DIGNDB.App.SmitteStop.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.API;
using DIGNDB.App.SmitteStop.API.Contracts;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class JwtValidationServiceTests
    {
        private IJwtValidationService _jwtValidationService;

        [SetUp]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCoreDependencies();
            serviceCollection.AddDALDependencies();
            serviceCollection.AddApiServiceCollectionDependencies();

            serviceCollection.AddScoped<HttpMessageHandler>(provider => CreateMockedHttpClientHandler());

            var jwtAuthorizationConfig = new JwtAuthorization
            {
                JwkUrl = "https://some-example-site.com",
                JwtValidationRules = new JwtValidationRules()
                {
                    ClientId = "smittestopp",
                    SupportedAlgorithm = "RS256",
                    Issuer = "https://dev-smittestopp-verification.azurewebsites.net"
                }
            };
            serviceCollection.AddSingleton(jwtAuthorizationConfig);

            serviceCollection.AddDbContext<DigNDB_SmittestopContext>(opts =>
                opts.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            _jwtValidationService = serviceProvider.GetService<IJwtValidationService>();
        }

        private const string ValidToken =
        "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjIifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDU3MTQyNSwiZXhwIjoxOTIxNDAwMjI1LCJjbGllbnRfaWQiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEifQ.WE5opqAN_SdENjAjeV85YsCOxdv0uQnTDAgsbdybWD_8G7LfoASV7hvJ8iHfiPzhfWQMq6L-XS6aai2yTWBiK5L2-BdnXsuEq_AtdDBtmomK7-jcOvKKce1QchA0d-3TILUgda5BCypCs8gi3jByfsP-Rjhm3hipc53tySYEOTqjBQF5pP3BNXkabH89Qez36m9268MIPf_m2IDzGVIKBY3Tfx5mf5EL5hLkJ47tzwLjbyTP8pVFPAASN23RVWRQJymYX3ytVF91cATvksRdz0HTCHzlB_eOBpNnzVBx0RHAkYHDpP3SsGcKKaL_S8_Oh1XcNHXkzieeyOVsxA_IqQ";

        [Test]
        public void TestValidateToken_WithValidToken()
        {
            var validationResult = _jwtValidationService.IsTokenValid(ValidToken);

            validationResult.Should().BeTrue();
        }

        [Test]
        public void TestValidateToken_WithValidTokenButInReplayAttack()
        {
            var validationResult = _jwtValidationService.IsTokenValid(ValidToken);
            validationResult.Should().BeTrue();

            Action validateAction = () => _jwtValidationService.IsTokenValid(ValidToken);

            validateAction.Should().Throw<SecurityTokenReplayDetectedException>()
                .WithMessage("The same token cannot be used again.*");
        }

        private const string TokenWithSignatureAlgorithmHS256 = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjIifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxOTIxNDAwMjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEifQ.A1paNTUnBadV84Q7JPFJDhrbapHaL1FGLgg9j1jhPWE";
        private const string TokenWithSignatureAlgorithmHS384 = "eyJhbGciOiJIUzM4NCIsInR5cCI6IkpXVCIsImtpZCI6IjIifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxOTIxNDAwMjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEifQ.6Ri19Ajfcry1qODNMb7q101omtcjISVTmk6pTwJ7PbBP67Ap2uBfk8j1hhlRw26s";
        private const string TokenWithSignatureAlgorithmHS512 = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCIsImtpZCI6IjIifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxOTIxNDAwMjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEifQ.j7UOn_pg4HrXo0xZKvqj1S0U_NmRnHDXmuKSxxgJK3dtgy6cwW6RQz07xNxefcuE5VYMfAhto0r4Gty1m3azEA";
        private const string TokenWithSignatureAlgorithmPS256 = "eyJhbGciOiJQUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjIifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxOTIxNDAwMjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEifQ.R7Cj1MhYvQ7FXP6H-0hU6Brj5gB9UOUNKMRvv0RVyuR5foKnBq2Lo_odZllmZkeBUWvtBMOTJ1obK_DGLscVEmE3YvglrDLyFEvlLqPuHWpXNEKBkpUn3LVWXFgqxWzDN16NAySYtSm4XFvHQTlibXUOF_bhjnfHkAjmNi-QtMrR7p82uYfduur8P5DjMmcyqblnLxG1nBxnDc4xJhSw5XP5ljTUfRQdNSprYCcSmhTZPgivDfUFlzX8XfwASW9mFpLll7APxn22KYXfSLjPY4wCUa4i6FWb58Xzl5XZMpMm4RyEkt6an65yiQnN8EWWNMnA6zpzzwv-FZEahl7DnA";
        private const string TokenWithSignatureAlgorithmPS384 = "eyJhbGciOiJQUzM4NCIsInR5cCI6IkpXVCIsImtpZCI6IjIifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxOTIxNDAwMjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEifQ.YpsZvq699LkO6V0xOMeVt4XWudLrtCH3G_hSN0-dll3s0Kl0M_VNO_st1-bRtCWMPq2MLwzuHVtwGILl6tAS8gXTfFLWfzz3DvO0ZU_9x6Ot1peSZ_mW5SL6kFrqDEVOFpRdCipOHsPBQ8Cv9chylTsQZtXmxfsZHnPUUSuv8VGW3o9boe7kcKz8B0tlfDRSzwrdrV4VqaIdH_g0x7r2_Xge_y-fGKavLQQj7L9jxAeyDyPLLPOuK81mM35EjcMKy1XyuXncI7QKSu2tN0It2sP6iLomk8gyRwXPvLh5s2KaPEMuYtLIlNSFqeAyB7GMG8fiAg5E54ziUgnBkdiQcQ";

        [TestCase(TokenWithSignatureAlgorithmHS256)]
        [TestCase(TokenWithSignatureAlgorithmHS384)]
        [TestCase(TokenWithSignatureAlgorithmHS512)]
        [TestCase(TokenWithSignatureAlgorithmPS256)]
        [TestCase(TokenWithSignatureAlgorithmPS384)]
        public void TestValidateToken_WithInvalidSignatureAlgorithm(string token)
        {
            Action validateAction = () => _jwtValidationService.IsTokenValid(token);

            validateAction.Should().Throw<NotSupportedException>()
                .WithMessage("Provided algorithm is not supported.*");
        }

        private const string TokenWithInvalidClientId =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjIifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxOTIxNDAwMjI1LCJjbGllbnRfaWQiOiJ4eHgiLCJpc3MiOiJodHRwczovL2Rldi1zbWl0dGVzdG9wcC12ZXJpZmljYXRpb24uYXp1cmV3ZWJzaXRlcy5uZXQiLCJqdGkiOiIxIn0.JHg3aECpa4L9p_EAL-oFQ069_-Vznht8H-RIb__5hfOfM_3rRQfGuX4G5o6PxGMpV7Qv9TniVTm8oi2BKZxZDYZT-oPfwrRMFikKULaKCQv6QcX62f1TsfyCE46t8c2U-xJQekbKpnK9qI9iHGl1cx7acDVWpwdl-3U4l9Df68IIKd3wvFQWDUlG8rE3imQt4pyWos_Cue4C98nxwXBi4Jqpqm0QMpLQmiDMKB_McXoy_o_6UhTjVDCuukBgOUd4eaDrjz26rRCa8QeoDUVc-eAjUsUdQwmIYp3f-bkuSXhJYg89c9CO5cceqsDDaFZGsAvEzwdxwEVRZiSZlDnSWA";

        [Test]
        public void TestValidateToken_WithInvalidClientId()
        {
            Action validateAction = () => _jwtValidationService.IsTokenValid(TokenWithInvalidClientId);

            validateAction.Should().Throw<SecurityTokenException>()
                .WithMessage("client_id claim is invalid.*");
        }

        private const string TokenWithExpiredTimeWindow =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjIifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxNjA0ODMwNjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEifQ.PCHkooTIfvF8WiU77oYzuBg_AqBOHIoSMDIwFzAU4BtsperCgWZ-aPp-TyMvbPjupb6E1bfE4lRbT4GtFIrPd27hxqBfTEvNE0yx1q0kX2sRV4oeqiDxI3q4UmJppN-MttY5SL0rxDCwJ9UzoZIYAWK1xNBY1WMsWRXlJoGon61BuNZghZJ_ZreXGhTULdyQYra5MrlmfP3Y3QkJH6_Nr-mvh5gmXUIXvYfm8yY9QkOu2dk8FNIME4fiWbx8TtxgE0G7FN5yczpbcQ_H-QbACjXMPDj6Owf-VwTTfKEpk5py0C3mjnTjo5FyjGEZFuhnKdETVUqe21WtI8fnkivFCg";
        private const string TokenIssuedInTheFuture =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjIifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNTQzNTQyNSwiZXhwIjoxNjA0ODMwNjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEifQ.fIibFcbOkcN7qUoS068kHyPGhI5uOKL5gepwV_wQXgl2McseXQX8bQY_mcMqly0nYv2ZFigw-YFvwy2fSs-UQY1pGfHlwDPX0lrpC7eHQywt5Gs1VpQivwwYBItaMhagtIL4g_RFrtOfk2VBSX4v_vFp00iEM7iM7hYbZ1d9bXHDIuzO9lUssD8sIcZVH51gYJPN3x3A0c19vZ0odjbgv-7_xPhw9r-vnNneMTB2NmwK_79FF4-W49qMpyYOBFi_h944_3k_OyIWebtPHqHm8Ywspa5D64v8fdVAaGXTsBtjzAg2tMY_nBXQazJj-9SEqAO1bm_0ST3sPQ89TohitQ";

        [TestCase(TokenWithExpiredTimeWindow)]
        [TestCase(TokenIssuedInTheFuture)]
        public void TestValidateToken_WithInvalidTimeWindow(string token)
        {
            Action validateAction = () => _jwtValidationService.IsTokenValid(token);

            validateAction.Should().Throw<SecurityTokenExpiredException>()
                .WithMessage("IDX10223: Lifetime validation failed. The token is expired.*");
        }

        private const string TokenWithNotKnownPublicKey =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IngifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDU3MTQyNSwiZXhwIjoxOTIxNDAwMjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEifQ.O_eFQbNkHdJhLwi0HXhi7HsYaApmQEugcyv0QRfiNvnGNS3gvy1dZIKcdjL1-99QExBIR1RPv3DpXQKfwM8LU1_lfNjZzJ11aVRt-D1WRoU8578wKmQ9VMVPZl9m23ikpjziZYyIRBvMJZk_qFNNAprFm1dylFDyH-gcO-uWPT22suE3T6J_PWWOjQUWrZ8kE2dby5Heo3Oc-C-6Jn4YxDqYy66p0A4izLf8m0bnqvc7CIw8zPR9n-23AaPLmwlPu025OquPZ4dcr-_6SXjYSI5OZyXQnOU6FH6xK2xn5CfCdejdYKrT5Q1YKH-7lmDHqF6vhDB_Bigm8Vtu_mkHZA";

        [Test]
        public void TestValidateToken_WithUnknownPublicKey()
        {
            Action validateAction = () => _jwtValidationService.IsTokenValid(TokenWithNotKnownPublicKey);

            validateAction.Should().Throw<RsaPublicKeyNotFoundException>()
                .WithMessage("Cannot find RSA public key for given key id.*");
        }

        private const string TokenWithInvalidIssuer =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjIifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDU3MTQyNSwiZXhwIjoxOTIxNDAwMjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8veHh4LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEifQ.TA4bKB9_26p6NkjtBLNfCysimjlPyV1NWEMPibUQUJ_V-kfn9ryDJyuCFozWO4-PBjrFG6WB3fbaeWLl7rH9jeoJ8jJVs_ZQNrzZR535iqyqNb26mq454R3a_h4h330Qg5VPPrBPFO7Ec59wyZ3ZDEtt7wD7mk-H-yLD11Cx3CQ11n9cgjVBy719zLJwizaJihe105zefRof50k_j0ytM4G-UdLx3gY4O9zyBnZaC7eLyijImDGRb9p38UgVXVYhroLLQaijPEbJREUrjM41F8v_RQMjopVGO2yUhvfY91p_mtkoWN2IQssBGbeHyCbnMuL6AlZ0Gm1dhdB0BMoahg";

        [Test]
        public void TestValidateToken_WithInvalidIssuer()
        {
            Action validateAction = () => _jwtValidationService.IsTokenValid(TokenWithInvalidIssuer);

            validateAction.Should().Throw<SecurityTokenInvalidIssuerException>()
                .WithMessage("IDX10205: Issuer validation failed.*");
        }

        private HttpClientHandler CreateMockedHttpClientHandler()
        {
            var expectedJsonWebKeyCollection = new JsonWebKeyCollection
            {
                Keys = new List<JsonWebKey>
                {
                    new JsonWebKey
                    {
                        Kid = "2",
                        N =
                            "AJ88orNWY3zQdGwYChTEr75E7cJbwbGiau0ucAPpM3lTlaVVsJFnVYWuLN/FzP6Wv8q+O2r+/s91U5rw0cgB3Gk/dsIURBaS7/XI+ZU3iUom8q/zK5v2LYwmVVoGjmCIcK18Ci6j6/9dYp1rAJHyMrbx1k8WWBHFy4AFxblLmkt7hfYBIjUMMxk1Nb9BapKkwa+AfJ1txwjeO11LtLfGNHvpX+LODsUGsFg+/Sff+Xd0ctL21dwJtRbRiYibzsEbCH1QoQ6WErU3B0wjKrb1m1ei9dQVpKcxl0luB7+N6mvhkmDg9kFOvDG+faEpNjgfgbTi6SaH5mxhBoL5sMgiPTM=",
                        E = "AQAB"
                    }
                }
            };

            var handlerMock = new Mock<HttpClientHandler>();
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(expectedJsonWebKeyCollection)),
            };

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            return handlerMock.Object;
        }
    }
}