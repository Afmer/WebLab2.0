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
    public DbSet<Feedback> Feedbacks {get; set;}
    public DbSet<FavoriteShow> FavoriteShows {get; set;}
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
        var firstShow = new Show
        {
            Id = Guid.NewGuid(),
            Name = "Звездные переплетения",
            Description = "\"Звездные переплетения\" - это захватывающее космическое приключение, " + 
            "где зрители отправятся в путешествие сквозь звездные системы и попадут в миры, где сбываются самые смелые мечты. " +
            "С помощью инновационной технологии виртуальной реальности, этот спектакль предлагает уникальный опыт взаимодействия с космическим пространством. " +
            "\"Звездные переплетения\" расскажут о важности мечтаний и веры в свои силы, даже когда все кажется невозможным.",
            Date = _getTimeNow + TimeSpan.FromDays(1),
            LabelImage = new Guid("e9aab09d-4cb6-4d13-808d-671fff8e701c")
        };
        var secondShow = new Show
        {
            Id = Guid.NewGuid(),
            Name = "Магия Востока: Заколдованный Лотос",
            Description = "\"Магия Востока: Заколдованный Лотос\" - это загадочное представление, полное волшебства, тайн и неожиданных сюжетных поворотов. " +
            "Зрители будут перенесены в древний восточный мир, где магия пронизывает каждый аспект жизни. " +
            "С помощью великолепных костюмов, декораций и потрясающих спецэффектов, этот спектакль погрузит вас в удивительный мир восточной культуры и древних тайн.",
            Date = _getTimeNow + TimeSpan.FromDays(2),
            LabelImage = new Guid("fcece7a5-914f-4881-8fd9-7ac3a93daada")
        };
        var thirdShow = new Show
        {
            Id = Guid.NewGuid(),
            Name = "Гармония Сезонов",
            Description = "\"Гармония Сезонов\" - это увлекательное представление, в котором каждый акт представляет собой новый сезон года. " +
            "От зимнего вихря до весеннего расцвета, от летнего зноя до осеннего умиротворения - каждый сезон преподносит свои удивительные сюрпризы и чудеса. " +
            "Этот спектакль наполнен эмоциями, музыкой и невероятными хореографическими номерами, которые погружают зрителей в магию перемен и красоту природы.",
            Date = _getTimeNow + TimeSpan.FromDays(2),
            LabelImage = new Guid("da00da3a-ee18-40bf-bd68-2cbd53d41b7d")
        };
        modelBuilder.Entity<Show>().HasData(
            firstShow,
            secondShow,
            thirdShow
        );
    }
}