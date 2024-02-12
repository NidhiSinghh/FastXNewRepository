namespace FastX.Interfaces
{
    public interface ISeatRepository<K,T>
    {
        public Task<T> GetAsync(K key1,K key2);
    }
}
