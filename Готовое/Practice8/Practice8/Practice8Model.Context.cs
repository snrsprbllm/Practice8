namespace Practice8
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class Practice8Entities1 : DbContext
    {
        // Конструктор по умолчанию
        public Practice8Entities1()
            : base("name=Practice8Entities1")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        // DbSets для ваших таблиц
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Сategories> Сategories { get; set; }

        // Статический метод для получения контекста
        private static Practice8Entities1 _context;

        public static Practice8Entities1 GetContext()
        {
            if (_context == null)
            {
                _context = new Practice8Entities1();
            }
            return _context;
        }
    }
}