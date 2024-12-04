public class Singleton
{
    private static Singleton instance;

    public static Singleton GetInstance()
    {
        if (instance == null)
            instance = new Singleton();
        
        return instance;
    }

    private Singleton() {}
}