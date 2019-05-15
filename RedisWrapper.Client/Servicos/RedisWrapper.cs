using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MsgPack.Serialization;
using RedisWarpper.Interface;

namespace RedisWrapper.Service.Servicos
{
    public class RedisWrapper : IRedisWrapper
    {
        private const int ExpiracaoPadrao = 1440;
        private const string RedisConnection = "localhost:6379";

        public class RedisStore
        {
            private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

            static RedisStore()
            {
                var configurationOptions = new ConfigurationOptions
                {
                    EndPoints = { ConfigurationManager.AppSettings["redis-connectionstring"] }
                };

                LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
            }

            public static ConnectionMultiplexer Connection => LazyConnection.Value;

            public static IDatabase RedisCache => Connection.GetDatabase();
        }

        /// <summary>
        /// Salva um objeto serializavel no cache do Redis
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="objeto">Instância do objeto</param>
        /// <param name="chave">Valor único utilizado para localizar</param>
        public void Salvar<T>(T objeto, string chave)
        {
            var db = RedisStore.RedisCache;

            var serializer = MessagePackSerializer.Get<T>();
            using (var stream = new MemoryStream())
            {
                serializer.Pack(stream, objeto);
                stream.Position = 0;

                db.StringSet(chave, stream.ToArray(), new TimeSpan(0, ExpiracaoPadrao, 0));
            }
        }

        /// <summary>
        /// Retorna um objeto salvo do cache do Redis
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="chave">Valor único utilizado para localizar</param>
        /// <returns>Objeto do tipo T</returns>
        public T Buscar<T>(string chave)
        {
            var db = RedisStore.RedisCache;

            var result = db.StringGet(chave);
            if (result.HasValue)
            {
                db.KeyExpire(chave, new TimeSpan(0, ExpiracaoPadrao, 0));

                var serializer = MessagePackSerializer.Get<T>();
                var dado = serializer.Unpack(new MemoryStream(result));
                return dado;
            }

            return default(T);
        }
    }
}
