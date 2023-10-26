using Microsoft.EntityFrameworkCore;
using Weblab.Architecture.Enums;
using Weblab.Modules.DB.DataModel;

namespace Weblab.Modules.DB;

public class ApplicationContext : DbContext
{
    public DbSet<PartialView> PartialViews {get; set;}
    public DbSet<MainPartialView> MainPartialViews {get; set;}
    public DbSet<UserIdentityInfo> UserIdentities {get; set;}
    public DbSet<Show> Shows {get; set;}
    private DateTime _getTimeNow => DateTime.UtcNow;
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
    :base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var time = _getTimeNow;
        var home = new MainPartialView
        {
            Id = MainPartialViewCode.Home,
            Html = @"        
            <div>
                <h1>hello</h1>
                <p>
                    Театральный клуб ""В Созвездиях"" приглашает вас в удивительный мир искусства под открытым небом.
                    Мы расположены в самом центре города и предлагаем незабвенные впечатления от разнообразных представлений.
                    Наши актеры, режиссеры и художники творят с любовью к искусству, создавая удивительные постановки - от классики до современности.
                    Помимо театральных представлений, мы организовываем мастер-классы, встречи с актерами и лекции по искусству.
                    Наш театр расположен в уютном парке, где зрители могут наслаждаться природой и искусством в едином гармоничном образе.
                    Присоединяйтесь к нам и погрузитесь в удивительный мир театра ""В Созвездиях"".
                    Наша команда с нетерпением ждет встречи с вами и обещает незабвенные впечатления!
                </p>
            </div>",
            DateUpdate = time
        };
        modelBuilder.Entity<MainPartialView>().HasData(
            home
        );
    }
}