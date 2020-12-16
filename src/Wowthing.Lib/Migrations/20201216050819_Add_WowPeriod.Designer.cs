﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Wowthing.Lib.Contexts;
using Wowthing.Lib.Models;

namespace Wowthing.Lib.Migrations
{
    [DbContext(typeof(WowDbContext))]
    [Migration("20201216050819_Add_WowPeriod")]
    partial class Add_WowPeriod
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnName("concurrency_stamp")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedName")
                        .HasColumnName("normalized_name")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_identity_role");

                    b.ToTable("asp_net_roles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<long>", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnName("concurrency_stamp")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnName("normalized_name")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnName("claim_type")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnName("claim_value")
                        .HasColumnType("text");

                    b.Property<long>("RoleId")
                        .HasColumnName("role_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id")
                        .HasName("pk_role_claims");

                    b.HasIndex("RoleId")
                        .HasName("ix_role_claims_role_id");

                    b.ToTable("asp_net_role_claims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnName("claim_type")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnName("claim_value")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id")
                        .HasName("pk_user_claims");

                    b.HasIndex("UserId")
                        .HasName("ix_user_claims_user_id");

                    b.ToTable("asp_net_user_claims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnName("login_provider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnName("provider_key")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnName("provider_display_name")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_user_logins");

                    b.HasIndex("UserId")
                        .HasName("ix_user_logins_user_id");

                    b.ToTable("asp_net_user_logins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint");

                    b.Property<long>("RoleId")
                        .HasColumnName("role_id")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_user_roles");

                    b.HasIndex("RoleId")
                        .HasName("ix_user_roles_role_id");

                    b.ToTable("asp_net_user_roles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint");

                    b.Property<string>("LoginProvider")
                        .HasColumnName("login_provider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnName("value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_user_tokens");

                    b.ToTable("asp_net_user_tokens");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.ApplicationUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnName("access_failed_count")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnName("concurrency_stamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnName("email_confirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnName("lockout_enabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnName("lockout_end")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnName("normalized_email")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnName("normalized_user_name")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnName("password_hash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnName("phone_number")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnName("phone_number_confirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnName("security_stamp")
                        .HasColumnType("text");

                    b.Property<ApplicationUserSettings>("Settings")
                        .HasColumnName("settings")
                        .HasColumnType("jsonb");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnName("two_factor_enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnName("user_name")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("asp_net_users");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("AccountId")
                        .HasColumnName("account_id")
                        .HasColumnType("bigint");

                    b.Property<bool>("Enabled")
                        .HasColumnName("enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<int>("Region")
                        .HasColumnName("region")
                        .HasColumnType("integer");

                    b.Property<string>("Tag")
                        .HasColumnName("tag")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id")
                        .HasName("pk_player_account");

                    b.HasIndex("UserId")
                        .HasName("ix_player_account_user_id");

                    b.HasIndex("Region", "AccountId")
                        .IsUnique()
                        .HasName("ix_player_account_region_account_id");

                    b.ToTable("player_account");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerCharacter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AccountId")
                        .HasColumnName("account_id")
                        .HasColumnType("integer");

                    b.Property<int>("ActiveSpecId")
                        .HasColumnName("active_spec_id")
                        .HasColumnType("integer");

                    b.Property<int>("ActiveTitleId")
                        .HasColumnName("active_title_id")
                        .HasColumnType("integer");

                    b.Property<int>("AverageItemLevel")
                        .HasColumnName("average_item_level")
                        .HasColumnType("integer");

                    b.Property<long>("CharacterId")
                        .HasColumnName("character_id")
                        .HasColumnType("bigint");

                    b.Property<int>("ClassId")
                        .HasColumnName("class_id")
                        .HasColumnType("integer");

                    b.Property<int>("DelayHours")
                        .HasColumnName("delay_hours")
                        .HasColumnType("integer");

                    b.Property<int>("EquippedItemLevel")
                        .HasColumnName("equipped_item_level")
                        .HasColumnType("integer");

                    b.Property<int>("Experience")
                        .HasColumnName("experience")
                        .HasColumnType("integer");

                    b.Property<int>("Faction")
                        .HasColumnName("faction")
                        .HasColumnType("integer");

                    b.Property<int>("Gender")
                        .HasColumnName("gender")
                        .HasColumnType("integer");

                    b.Property<long>("GuildId")
                        .HasColumnName("guild_id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("LastApiCheck")
                        .HasColumnName("last_api_check")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Level")
                        .HasColumnName("level")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<int>("RaceId")
                        .HasColumnName("race_id")
                        .HasColumnType("integer");

                    b.Property<int>("RealmId")
                        .HasColumnName("realm_id")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("pk_player_character");

                    b.HasIndex("AccountId")
                        .HasName("ix_player_character_account_id");

                    b.HasIndex("RealmId", "Name")
                        .IsUnique()
                        .HasName("ix_player_character_realm_id_name");

                    b.ToTable("player_character");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerCharacterEquippedItem", b =>
                {
                    b.Property<int>("CharacterId")
                        .HasColumnName("character_id")
                        .HasColumnType("integer");

                    b.Property<int>("InventorySlot")
                        .HasColumnName("inventory_slot")
                        .HasColumnType("integer");

                    b.Property<List<int>>("BonusIds")
                        .HasColumnName("bonus_ids")
                        .HasColumnType("integer[]");

                    b.Property<int>("Context")
                        .HasColumnName("context")
                        .HasColumnType("integer");

                    b.Property<List<int>>("EnchantmentIds")
                        .HasColumnName("enchantment_ids")
                        .HasColumnType("integer[]");

                    b.Property<int>("ItemId")
                        .HasColumnName("item_id")
                        .HasColumnType("integer");

                    b.Property<int>("ItemLevel")
                        .HasColumnName("item_level")
                        .HasColumnType("integer");

                    b.Property<int>("Quality")
                        .HasColumnName("quality")
                        .HasColumnType("integer");

                    b.HasKey("CharacterId", "InventorySlot")
                        .HasName("pk_player_character_equipped_item");

                    b.ToTable("player_character_equipped_item");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerCharacterQuests", b =>
                {
                    b.Property<int>("CharacterId")
                        .HasColumnName("character_id")
                        .HasColumnType("integer");

                    b.Property<List<int>>("CompletedIds")
                        .HasColumnName("completed_ids")
                        .HasColumnType("integer[]");

                    b.HasKey("CharacterId")
                        .HasName("pk_player_character_quests");

                    b.ToTable("player_character_quests");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerCharacterReputations", b =>
                {
                    b.Property<int>("CharacterId")
                        .HasColumnName("character_id")
                        .HasColumnType("integer");

                    b.Property<List<int>>("ReputationIds")
                        .HasColumnName("reputation_ids")
                        .HasColumnType("integer[]");

                    b.Property<List<int>>("ReputationValues")
                        .HasColumnName("reputation_values")
                        .HasColumnType("integer[]");

                    b.HasKey("CharacterId")
                        .HasName("pk_player_character_reputations");

                    b.ToTable("player_character_reputations");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerCharacterShadowlands", b =>
                {
                    b.Property<int>("CharacterId")
                        .HasColumnName("character_id")
                        .HasColumnType("integer");

                    b.Property<List<int>>("ConduitIds")
                        .HasColumnName("conduit_ids")
                        .HasColumnType("integer[]");

                    b.Property<List<int>>("ConduitRanks")
                        .HasColumnName("conduit_ranks")
                        .HasColumnType("integer[]");

                    b.Property<int>("CovenantId")
                        .HasColumnName("covenant_id")
                        .HasColumnType("integer");

                    b.Property<int>("RenownLevel")
                        .HasColumnName("renown_level")
                        .HasColumnType("integer");

                    b.Property<int>("SoulbindId")
                        .HasColumnName("soulbind_id")
                        .HasColumnType("integer");

                    b.HasKey("CharacterId")
                        .HasName("pk_player_character_shadowlands");

                    b.ToTable("player_character_shadowlands");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.Query.SchedulerCharacterQuery", b =>
                {
                    b.Property<int>("CharacterId")
                        .HasColumnName("character_id")
                        .HasColumnType("integer");

                    b.Property<string>("CharacterName")
                        .HasColumnName("character_name")
                        .HasColumnType("text");

                    b.Property<string>("RealmSlug")
                        .HasColumnName("realm_slug")
                        .HasColumnType("text");

                    b.Property<int>("Region")
                        .HasColumnName("region")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint");

                    b.ToTable("scheduler_character_query");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid");

                    b.Property<int>("DefaultRealmId")
                        .HasColumnName("default_realm_id")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<int>("Region")
                        .HasColumnName("region")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("pk_team");

                    b.ToTable("team");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.TeamCharacter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CharacterId")
                        .HasColumnName("character_id")
                        .HasColumnType("integer");

                    b.Property<string>("Note")
                        .HasColumnName("note")
                        .HasColumnType("text");

                    b.Property<int>("PrimaryRole")
                        .HasColumnName("primary_role")
                        .HasColumnType("integer");

                    b.Property<int>("SecondaryRole")
                        .HasColumnName("secondary_role")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TeamId")
                        .HasColumnName("team_id")
                        .HasColumnType("uuid");

                    b.HasKey("Id")
                        .HasName("pk_team_character");

                    b.HasIndex("CharacterId")
                        .HasName("ix_team_character_character_id");

                    b.HasIndex("TeamId")
                        .HasName("ix_team_character_team_id");

                    b.ToTable("team_character");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.WowClass", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("id")
                        .HasColumnType("integer");

                    b.Property<string>("Icon")
                        .HasColumnName("icon")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<List<int>>("SpecializationIds")
                        .HasColumnName("specialization_ids")
                        .HasColumnType("integer[]");

                    b.HasKey("Id")
                        .HasName("pk_wow_class");

                    b.ToTable("wow_class");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.WowPeriod", b =>
                {
                    b.Property<int>("Region")
                        .HasColumnName("region")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnName("id")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Ends")
                        .HasColumnName("ends")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Starts")
                        .HasColumnName("starts")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Region", "Id")
                        .HasName("pk_wow_period");

                    b.ToTable("wow_period");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.WowRace", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("id")
                        .HasColumnType("integer");

                    b.Property<string>("IconFemale")
                        .HasColumnName("icon_female")
                        .HasColumnType("text");

                    b.Property<string>("IconMale")
                        .HasColumnName("icon_male")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_wow_race");

                    b.ToTable("wow_race");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.WowRealm", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<int>("Region")
                        .HasColumnName("region")
                        .HasColumnType("integer");

                    b.Property<string>("Slug")
                        .HasColumnName("slug")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_wow_realm");

                    b.ToTable("wow_realm");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.WowReputation", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<int>("TierId")
                        .HasColumnName("tier_id")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("pk_wow_reputation");

                    b.ToTable("wow_reputation");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.WowReputationTier", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("id")
                        .HasColumnType("integer");

                    b.Property<int[]>("MaxValues")
                        .HasColumnName("max_values")
                        .HasColumnType("integer[]");

                    b.Property<int[]>("MinValues")
                        .HasColumnName("min_values")
                        .HasColumnType("integer[]");

                    b.Property<string[]>("Names")
                        .HasColumnName("names")
                        .HasColumnType("text[]");

                    b.HasKey("Id")
                        .HasName("pk_wow_reputation_tier");

                    b.ToTable("wow_reputation_tier");
                });

            modelBuilder.Entity("Wowthing.Lib.Models.WowTitle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<string>("TitleFemale")
                        .HasColumnName("title_female")
                        .HasColumnType("text");

                    b.Property<string>("TitleMale")
                        .HasColumnName("title_male")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_wow_title");

                    b.ToTable("wow_title");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<long>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_role_claims_asp_net_roles_identity_role_long_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("Wowthing.Lib.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_claims_asp_net_users_application_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("Wowthing.Lib.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_logins_asp_net_users_application_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<long>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_user_roles_asp_net_roles_identity_role_long_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wowthing.Lib.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_roles_asp_net_users_application_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.HasOne("Wowthing.Lib.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_tokens_asp_net_users_application_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerAccount", b =>
                {
                    b.HasOne("Wowthing.Lib.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_player_account_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerCharacter", b =>
                {
                    b.HasOne("Wowthing.Lib.Models.PlayerAccount", "Account")
                        .WithMany("Characters")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("fk_player_character_player_account_account_id")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerCharacterEquippedItem", b =>
                {
                    b.HasOne("Wowthing.Lib.Models.PlayerCharacter", "Character")
                        .WithMany("EquippedItems")
                        .HasForeignKey("CharacterId")
                        .HasConstraintName("fk_player_character_equipped_item_player_character_character_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerCharacterQuests", b =>
                {
                    b.HasOne("Wowthing.Lib.Models.PlayerCharacter", "Character")
                        .WithOne("Quests")
                        .HasForeignKey("Wowthing.Lib.Models.PlayerCharacterQuests", "CharacterId")
                        .HasConstraintName("fk_player_character_quests_player_character_character_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerCharacterReputations", b =>
                {
                    b.HasOne("Wowthing.Lib.Models.PlayerCharacter", "Character")
                        .WithOne("Reputations")
                        .HasForeignKey("Wowthing.Lib.Models.PlayerCharacterReputations", "CharacterId")
                        .HasConstraintName("fk_player_character_reputations_player_character_character_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wowthing.Lib.Models.PlayerCharacterShadowlands", b =>
                {
                    b.HasOne("Wowthing.Lib.Models.PlayerCharacter", "Character")
                        .WithOne("Shadowlands")
                        .HasForeignKey("Wowthing.Lib.Models.PlayerCharacterShadowlands", "CharacterId")
                        .HasConstraintName("fk_player_character_shadowlands_player_character_character_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wowthing.Lib.Models.TeamCharacter", b =>
                {
                    b.HasOne("Wowthing.Lib.Models.PlayerCharacter", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterId")
                        .HasConstraintName("fk_team_character_player_character_character_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wowthing.Lib.Models.Team", null)
                        .WithMany("Characters")
                        .HasForeignKey("TeamId")
                        .HasConstraintName("fk_team_character_team_team_id");
                });
#pragma warning restore 612, 618
        }
    }
}
