namespace MlSuite.MlSynch
{
    public static class CallbackSemaphore
    {
        public static SemaphoreSlim semaphore = new SemaphoreSlim(1);
    }
}
