
public class TableauManager : IInjectable
{
    private Tableau m_currentTableau = null;
    public Tableau CurrentTableau => m_currentTableau;

    public TableauManager()
    {
        InjectionManager.Register(this);
    }

    public Tableau NewTableau()
    {
        m_currentTableau = new Tableau();
        return m_currentTableau;
    }
}