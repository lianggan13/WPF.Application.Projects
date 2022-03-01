using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartParking.Server.Model;

namespace SmartParking.Server.DAL.EFCore
{
    public class MySqlDbContext : DbContext
    {

        public MySqlDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 联合主键设置
            modelBuilder.Entity<SmartParking.Server.Model.qrtz_blob_triggers>().HasKey(
                    t => new { t.SCHED_NAME, t.TRIGGER_NAME, t.TRIGGER_GROUP });

            modelBuilder.Entity<SmartParking.Server.Model.qrtz_calendars>().HasKey(
                    t => new { t.SCHED_NAME, t.CALENDAR_NAME });

            modelBuilder.Entity<SmartParking.Server.Model.qrtz_cron_triggers>().HasKey(
                    t => new { t.SCHED_NAME, t.TRIGGER_NAME, t.TRIGGER_GROUP });

            modelBuilder.Entity<SmartParking.Server.Model.qrtz_fired_triggers>().HasKey(
                    t => new { t.SCHED_NAME, t.TRIGGER_NAME, t.TRIGGER_GROUP });

            modelBuilder.Entity<SmartParking.Server.Model.qrtz_job_details>().HasKey(
                t => new { t.SCHED_NAME, t.JOB_NAME, t.JOB_GROUP });

            modelBuilder.Entity<SmartParking.Server.Model.qrtz_locks>().HasKey(
               t => new { t.SCHED_NAME, t.LOCK_NAME });

            modelBuilder.Entity<SmartParking.Server.Model.qrtz_paused_trigger_grps>().HasKey(
              t => new { t.SCHED_NAME, t.TRIGGER_GROUP });

            modelBuilder.Entity<SmartParking.Server.Model.qrtz_scheduler_state>().HasKey(
              t => new { t.SCHED_NAME, t.INSTANCE_NAME });

            modelBuilder.Entity<SmartParking.Server.Model.qrtz_simple_triggers>().HasKey(
              t => new { t.SCHED_NAME, t.TRIGGER_NAME, t.TRIGGER_GROUP });

            modelBuilder.Entity<SmartParking.Server.Model.qrtz_simprop_triggers>().HasKey(
               t => new { t.SCHED_NAME, t.TRIGGER_NAME, t.TRIGGER_GROUP });

            modelBuilder.Entity<SmartParking.Server.Model.qrtz_triggers>().HasKey(
            t => new { t.SCHED_NAME, t.TRIGGER_NAME, t.TRIGGER_GROUP });

            // 菜单表中的字体图标值转换
            ValueConverter iconValueConverter = new ValueConverter<string, string>(v => ConvertToProvider(v), v => ConvertFromProvider(v));

            modelBuilder.Entity<sys_menu>().Property(p => p.icon).HasConversion(iconValueConverter);
        }

        public string ConvertToProvider(string v)
        {
            string res = string.IsNullOrEmpty(v) ? null : ((int)v.ToCharArray()[0]).ToString("x");
            return res;
        }

        public string ConvertFromProvider(string v)
        {
            // "&#xe777;" 是Unicode编码
            // WPF 后台绑定,需要这样写: "\xe777" 
            string prefix = "&#x";
            if (!string.IsNullOrEmpty(v) && v.StartsWith(prefix))
            {
                v = v.Replace(prefix, "").Replace(";", "");
                v = ((char)int.Parse(v, NumberStyles.HexNumber)).ToString();
            }

            return v;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<qrtz_blob_triggers> qrtz_blob_triggers { get; set; }
        public virtual DbSet<qrtz_calendars> qrtz_calendars { get; set; }
        public virtual DbSet<qrtz_cron_triggers> qrtz_cron_triggers { get; set; }
        public virtual DbSet<qrtz_fired_triggers> qrtz_fired_triggers { get; set; }
        public virtual DbSet<qrtz_job_details> qrtz_job_details { get; set; }
        public virtual DbSet<qrtz_locks> qrtz_locks { get; set; }
        public virtual DbSet<qrtz_paused_trigger_grps> qrtz_paused_trigger_grps { get; set; }
        public virtual DbSet<qrtz_scheduler_state> qrtz_scheduler_state { get; set; }
        public virtual DbSet<qrtz_simple_triggers> qrtz_simple_triggers { get; set; }
        public virtual DbSet<qrtz_simprop_triggers> qrtz_simprop_triggers { get; set; }
        public virtual DbSet<qrtz_triggers> qrtz_triggers { get; set; }
        public virtual DbSet<schedule_job> schedule_job { get; set; }
        public virtual DbSet<schedule_job_log> schedule_job_log { get; set; }
        public virtual DbSet<sys_captcha> sys_captcha { get; set; }
        public virtual DbSet<sys_config> sys_config { get; set; }
        public virtual DbSet<sys_fingervalidate> sys_fingervalidate { get; set; }
        public virtual DbSet<sys_log> sys_log { get; set; }
        public virtual DbSet<sys_menu> sys_menu { get; set; }
        public virtual DbSet<sys_oss> sys_oss { get; set; }
        public virtual DbSet<sys_request_log> sys_request_log { get; set; }
        public virtual DbSet<sys_role> sys_role { get; set; }
        public virtual DbSet<sys_role_menu> sys_role_menu { get; set; }
        public virtual DbSet<sys_user> sys_user { get; set; }
        public virtual DbSet<sys_user_role> sys_user_role { get; set; }
        public virtual DbSet<sys_user_token> sys_user_token { get; set; }
        public virtual DbSet<sys_userface> sys_userface { get; set; }
        public virtual DbSet<sys_userfinger> sys_userfinger { get; set; }
        public virtual DbSet<t_tools_cabinet> t_tools_cabinet { get; set; }
        public virtual DbSet<t_tools_info> t_tools_info { get; set; }
        public virtual DbSet<t_tools_user> t_tools_user { get; set; }
        public virtual DbSet<tb_user> tb_user { get; set; }

    }
}
