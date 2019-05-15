namespace RedisWarpper.Interface
{
    public interface IRedisWrapper
    {
        void Salvar<T>(T objeto, string chave);
        T Buscar<T>(string chave);
    }
}
