using System;
using System.Collections.Generic;
using LinkNodeDomain.Model;
using Microsoft.EntityFrameworkCore;

namespace LinkNodeInfrastructure;

public partial class DbLinkNodeContext : DbContext
{
    public DbLinkNodeContext()
    {
    }

    public DbLinkNodeContext(DbContextOptions<DbLinkNodeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActionType> ActionTypes { get; set; }

    public virtual DbSet<AdminAction> AdminActions { get; set; }

    public virtual DbSet<CallStatusStory> CallStatusStories { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<EmploymentType> EmploymentTypes { get; set; }

    public virtual DbSet<Freelancer> Freelancers { get; set; }

    public virtual DbSet<Interview> Interviews { get; set; }

    public virtual DbSet<IntroStatus> IntroStatuses { get; set; }

    public virtual DbSet<Invite> Invites { get; set; }

    public virtual DbSet<InviteStatus> InviteStatuses { get; set; }

    public virtual DbSet<Proposal> Proposals { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Vacancy> Vacancies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Freelance;Username=yuliana;Password=postgres;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ActionTypes_pkey");

            entity.Property(e => e.Id).HasColumnName("actionId");
            entity.Property(e => e.Action).HasColumnName("action");
        });

        modelBuilder.Entity<AdminAction>(entity =>
        {
            entity.ToTable("AdminActions");
            entity.HasKey(e => e.Id).HasName("AdminActions_pkey");
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.AdminId)
                .HasColumnName("adminId");
            entity.Property(e => e.ActionId).HasColumnName("actionId");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdDate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.TargetUserId).HasColumnName("targetUserId");
            entity.Property(e => e.TargetVacancyId).HasColumnName("targetVacancyId");

            entity.HasOne(d => d.Action).WithMany(p => p.AdminActions)
                .HasForeignKey(d => d.ActionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdminActions_actionId_fkey");

            entity.HasOne(d => d.Admin).WithMany(p => p.AdminActions)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdminActions_adminId_fkey");

            entity.HasOne(d => d.TargetUser).WithMany(p => p.AdminTargetUsers)
                .HasForeignKey(d => d.TargetUserId)
                .HasConstraintName("AdminActions_targetUserId_fkey");

            entity.HasOne(d => d.TargetVacancy).WithMany(p => p.AdminActions)
                .HasForeignKey(d => d.TargetVacancyId)
                .HasConstraintName("AdminActions_targetVacancyId_fkey");
        });

        modelBuilder.Entity<CallStatusStory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CallStatusStories_pkey");

            entity.Property(e => e.Id).HasColumnName("storyId");
            entity.Property(e => e.ChangedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("changedDate");
            entity.Property(e => e.IntroId).HasColumnName("introId");
            entity.Property(e => e.NewStatusId).HasColumnName("newStatusId");
            entity.Property(e => e.OldStatusId).HasColumnName("oldStatusId");

            entity.HasOne(d => d.Intro).WithMany(p => p.CallStatusStories)
                .HasForeignKey(d => d.IntroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CallStatusStories_introId_fkey");

            entity.HasOne(d => d.NewStatus).WithMany(p => p.CallStatusStoryNewStatuses)
                .HasForeignKey(d => d.NewStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CallStatusStories_newStatusId_fkey");

            entity.HasOne(d => d.OldStatus).WithMany(p => p.CallStatusStoryOldStatuses)
                .HasForeignKey(d => d.OldStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CallStatusStories_oldStatusId_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Categories_pkey");

            entity.Property(e => e.Id).HasColumnName("categoryId");
            entity.Property(e => e.Category1).HasColumnName("category");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Clients_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("clientId");
            entity.Property(e => e.AvgHourlyRatePaid)
                .HasPrecision(10, 2)
                .HasColumnName("avgHourlyRatePaid");
            entity.Property(e => e.CompanyName).HasColumnName("companyName");
            entity.Property(e => e.HireRate).HasColumnName("hireRate");

            entity.HasOne(d => d.ClientNavigation).WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Clients_userId_fkey");
        });

        modelBuilder.Entity<EmploymentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EmploymentType_pkey");

            entity.Property(e => e.Id).HasColumnName("empTypeId");
            entity.Property(e => e.EmpType).HasColumnName("empType");
        });

        modelBuilder.Entity<Freelancer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Freelancers_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("freelancerId");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EmpTypeId).HasColumnName("empTypeId");
            entity.Property(e => e.HourlyRate)
                .HasPrecision(10, 2)
                .HasColumnName("hourlyRate");

            entity.HasOne(d => d.Category).WithMany(p => p.Freelancers)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Freelancers_categoryId_fkey");

            entity.HasOne(d => d.EmpType).WithMany(p => p.Freelancers)
                .HasForeignKey(d => d.EmpTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Freelancers_empTypeId_fkey");

            entity.HasOne(d => d.FreelancerNavigation).WithOne(p => p.Freelancer)
                .HasForeignKey<Freelancer>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Freelancers_userId_fkey");
        });

        modelBuilder.Entity<Interview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Interview_pkey");

            entity.Property(e => e.Id).HasColumnName("introId");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdDate");
            entity.Property(e => e.DateTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateTime");
            entity.Property(e => e.InterviewRound).HasColumnName("interviewRound");
            entity.Property(e => e.IntroStatusId).HasColumnName("introStatusId");
            entity.Property(e => e.PropId).HasColumnName("propId");
            entity.Property(e => e.Reference).HasColumnName("reference");

            entity.HasOne(d => d.IntroStatus).WithMany(p => p.Interviews)
                .HasForeignKey(d => d.IntroStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Interviews_introStatusId_fkey");

            entity.HasOne(d => d.Prop).WithMany(p => p.Interviews)
                .HasForeignKey(d => d.PropId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Interview_propId_fkey");
        });

        modelBuilder.Entity<IntroStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("IntroStatuses_pkey");

            entity.Property(e => e.Id).HasColumnName("introStatusId");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Invite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Invites_pkey");

            entity.Property(e => e.Id).HasColumnName("inviteId");
            entity.Property(e => e.FreelancerId).HasColumnName("freelancerId");
            entity.Property(e => e.StatusId).HasColumnName("statusId");
            entity.Property(e => e.VacancyId).HasColumnName("vacancyId");

            entity.HasOne(d => d.Freelancer).WithMany(p => p.Invites)
                .HasForeignKey(d => d.FreelancerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Invites_freelancerId_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Invites)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Invites_statusId_fkey");

            entity.HasOne(d => d.Vacancy).WithMany(p => p.Invites)
                .HasForeignKey(d => d.VacancyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Invites_vacancyId_fkey");
        });

        modelBuilder.Entity<InviteStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("InviteStatus_pkey");

            entity.Property(e => e.Id).HasColumnName("statusId");
            entity.Property(e => e.InviteStatus1).HasColumnName("inviteStatus");
        });

        modelBuilder.Entity<Proposal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Proposals_pkey");

            entity.Property(e => e.Id).HasColumnName("propId");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdDate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FreelancerId).HasColumnName("freelancerId");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.VacancyId).HasColumnName("vacancyId");

            entity.HasOne(d => d.Freelancer).WithMany(p => p.Proposals)
                .HasForeignKey(d => d.FreelancerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Proposals_freelancerId_fkey");

            entity.HasOne(d => d.Vacancy).WithMany(p => p.Proposals)
                .HasForeignKey(d => d.VacancyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Proposals_vacancyId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.Property(e => e.Id).HasColumnName("userId");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdDate");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.Surname).HasColumnName("surname");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedDate");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_roleId_fkey");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserRoles_pkey");

            entity.Property(e => e.Id).HasColumnName("roleId");
            entity.Property(e => e.Role).HasColumnName("role");
        });

        modelBuilder.Entity<Vacancy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Vacancies_pkey");

            entity.Property(e => e.Id).HasColumnName("vacancyId");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.ClientId).HasColumnName("clientId");
            entity.Property(e => e.ClosedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("closedDate");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdDate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EmpTypeId).HasColumnName("empTypeId");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.Category).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vacancies_categoryId_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vacancies_clientId_fkey");

            entity.HasOne(d => d.EmpType).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.EmpTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vacancies_empTypeId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
