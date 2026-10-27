using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillHubeduardo.Domain.Entities;

namespace SkillHubeduardo.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(SkillHubeduardoDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        // 1. Ensure Roles
        string[] roles = { "Employee", "Participant" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. Seed Instructors (Employees) from Excel
        await EnsureUserAsync(userManager, "andres.mejia@correo.com", "Instructor2026*", "Andrés Felipe Mejía Arango", "Employee", "Fotografía");
        await EnsureUserAsync(userManager, "rosa.barrios@correo.com", "Instructor2026*", "Rosa Elena Barrios Polo", "Employee", "Cocina");
        await EnsureUserAsync(userManager, "mauricio.jimenez@correo.com", "Instructor2026*", "Mauricio Jiménez Cárdenas", "Employee", "Oratoria");
        await EnsureUserAsync(userManager, "natalia.herrera@correo.com", "Instructor2026*", "Natalia Herrera Castaño", "Employee", "Diseño gráfico");
        await EnsureUserAsync(userManager, "hernan.quintero@correo.com", "Instructor2026*", "Hernán Darío Quintero Gil", "Employee", "Música");
        await EnsureUserAsync(userManager, "paola.vargas@correo.com", "Instructor2026*", "Paola Andrea Vargas Mora", "Employee", "Marketing digital");

        // 3. Seed Participants from Excel
        await EnsureUserAsync(userManager, "jorge.perez@correo.com", "Skillhub2026*", "Jorge Andrés Pérez Rojas", "Participant", null);
        await EnsureUserAsync(userManager, "luz.ospina@correo.com", "Skillhub2026*", "Luz Marina Ospina Ríos", "Participant", null);
        await EnsureUserAsync(userManager, "carlos.rincon@correo.com", "Skillhub2026*", "Carlos Eduardo Rincón Díaz", "Participant", null);
        await EnsureUserAsync(userManager, "valentina.gomez@correo.com", "Skillhub2026*", "Valentina Gómez Ruiz", "Participant", null);
        await EnsureUserAsync(userManager, "juan.restrepo@correo.com", "Skillhub2026*", "Juan Camilo Restrepo Vélez", "Participant", null);
        await EnsureUserAsync(userManager, "maria.torres@correo.com", "Skillhub2026*", "María Fernanda Torres León", "Participant", null);
        await EnsureUserAsync(userManager, "santiago.ramirez@correo.com", "Skillhub2026*", "Santiago Ramírez Henao", "Participant", null);
        await EnsureUserAsync(userManager, "daniela.morales@correo.com", "Skillhub2026*", "Daniela Morales Pineda", "Participant", null);
        await EnsureUserAsync(userManager, "sebastian.castro@correo.com", "Skillhub2026*", "Sebastián Castro Duque", "Participant", null);
        await EnsureUserAsync(userManager, "laura.martinez@correo.com", "Skillhub2026*", "Laura Sofía Martínez Suárez", "Participant", null);
        await EnsureUserAsync(userManager, "andres.salazar@correo.com", "Skillhub2026*", "Andrés Mauricio Salazar Ortiz", "Participant", null);
        await EnsureUserAsync(userManager, "camila.cardenas@correo.com", "Skillhub2026*", "Camila Andrea Cárdenas Ruiz", "Participant", null);
        await EnsureUserAsync(userManager, "diego.munoz@correo.com", "Skillhub2026*", "Diego Alejandro Muñoz Peña", "Participant", null);
        await EnsureUserAsync(userManager, "juliana.ortega@correo.com", "Skillhub2026*", "Juliana Ortega Sánchez", "Participant", null);
        await EnsureUserAsync(userManager, "felipe.agudelo@correo.com", "Skillhub2026*", "Felipe Agudelo Marín", "Participant", null);
        await EnsureUserAsync(userManager, "natalia.montoya@correo.com", "Skillhub2026*", "Natalia Montoya Correa", "Participant", null);
        await EnsureUserAsync(userManager, "oscar.guerrero@correo.com", "Skillhub2026*", "Óscar Iván Guerrero Lozano", "Participant", null);
        await EnsureUserAsync(userManager, "paula.zapata@correo.com", "Skillhub2026*", "Paula Andrea Zapata Ríos", "Participant", null);
        await EnsureUserAsync(userManager, "cristian.becerra@correo.com", "Skillhub2026*", "Cristian David Becerra Mejía", "Participant", null);
        await EnsureUserAsync(userManager, "mariana.londono@correo.com", "Skillhub2026*", "Mariana Londoño Álvarez", "Participant", null);
        await EnsureUserAsync(userManager, "javier.cuellar@correo.com", "Skillhub2026*", "Javier Alonso Cuéllar Pardo", "Participant", null);
        await EnsureUserAsync(userManager, "isabella.navarro@correo.com", "Skillhub2026*", "Isabella Navarro Fuentes", "Participant", null);
        await EnsureUserAsync(userManager, "kevin.mosquera@correo.com", "Skillhub2026*", "Kevin Stiven Mosquera Palacios", "Participant", null);
        await EnsureUserAsync(userManager, "ana.jaramillo@correo.com", "Skillhub2026*", "Ana María Jaramillo Gaviria", "Participant", null);
        await EnsureUserAsync(userManager, "luis.arias@correo.com", "Skillhub2026*", "Luis Fernando Arias Cano", "Participant", null);
        await EnsureUserAsync(userManager, "sara.valencia@correo.com", "Skillhub2026*", "Sara Valencia Bermúdez", "Participant", null);
        await EnsureUserAsync(userManager, "esteban.rodriguez@correo.com", "Skillhub2026*", "Esteban Rodríguez Molina", "Participant", null);
        await EnsureUserAsync(userManager, "manuela.escobar@correo.com", "Skillhub2026*", "Manuela Escobar Toro", "Participant", null);
        await EnsureUserAsync(userManager, "wilmer.benitez@correo.com", "Skillhub2026*", "Wilmer Alfonso Benítez Cruz", "Participant", null);
        await EnsureUserAsync(userManager, "yuliana.patino@correo.com", "Skillhub2026*", "Yuliana Patiño Osorio", "Participant", null);

        // 4. Seed Workshops from Excel
        await EnsureWorkshopAsync(context, userManager, "FOT-001", "Fotografía con celular", "Composición, luz y edición básica desde el celular", "andres.mejia@correo.com", DateTime.Parse("2026-12-05T08:00:00"), DateTime.Parse("2026-12-05T12:00:00"), 15);
        await EnsureWorkshopAsync(context, userManager, "FOT-002", "Retrato en exteriores", "Manejo de luz natural para retratos al aire libre", "andres.mejia@correo.com", DateTime.Parse("2027-01-16T08:00:00"), DateTime.Parse("2027-01-16T12:00:00"), 10);
        await EnsureWorkshopAsync(context, userManager, "FOT-003", "Edición en Lightroom", "Flujo de revelado y ajustes de color", "andres.mejia@correo.com", DateTime.Parse("2027-02-06T14:00:00"), DateTime.Parse("2027-02-06T18:00:00"), 12);
        await EnsureWorkshopAsync(context, userManager, "COC-001", "Cocina costeña", "Arroz con coco, patacones y mojarra frita", "rosa.barrios@correo.com", DateTime.Parse("2026-12-05T14:00:00"), DateTime.Parse("2026-12-05T18:00:00"), 12);
        await EnsureWorkshopAsync(context, userManager, "COC-002", "Repostería básica", "Tortas, galletas y postres de nevera", "rosa.barrios@correo.com", DateTime.Parse("2027-01-09T09:00:00"), DateTime.Parse("2027-01-09T13:00:00"), 10);
        await EnsureWorkshopAsync(context, userManager, "COC-003", "Cocina saludable", "Platos balanceados para la semana", "rosa.barrios@correo.com", DateTime.Parse("2027-02-13T09:00:00"), DateTime.Parse("2027-02-13T12:00:00"), 14);
        await EnsureWorkshopAsync(context, userManager, "ORA-001", "Oratoria para principiantes", "Técnicas para hablar en público con seguridad", "mauricio.jimenez@correo.com", DateTime.Parse("2026-12-12T09:00:00"), DateTime.Parse("2026-12-12T12:00:00"), 20);
        await EnsureWorkshopAsync(context, userManager, "ORA-002", "Presentaciones efectivas", "Cómo estructurar y presentar una idea en 10 minutos", "mauricio.jimenez@correo.com", DateTime.Parse("2027-01-23T09:00:00"), DateTime.Parse("2027-01-23T12:00:00"), 18);
        await EnsureWorkshopAsync(context, userManager, "DIS-001", "Diseño en Canva", "Piezas gráficas para redes sociales", "natalia.herrera@correo.com", DateTime.Parse("2026-12-19T14:00:00"), DateTime.Parse("2026-12-19T17:00:00"), 18);
        await EnsureWorkshopAsync(context, userManager, "DIS-002", "Fundamentos de color y tipografía", "Principios visuales para piezas profesionales", "natalia.herrera@correo.com", DateTime.Parse("2027-01-30T14:00:00"), DateTime.Parse("2027-01-30T17:00:00"), 15);
        await EnsureWorkshopAsync(context, userManager, "MUS-001", "Guitarra desde cero", "Acordes básicos y primeras canciones", "hernan.quintero@correo.com", DateTime.Parse("2026-12-12T14:00:00"), DateTime.Parse("2026-12-12T17:00:00"), 10);
        await EnsureWorkshopAsync(context, userManager, "MUS-002", "Producción musical en casa", "Grabación y mezcla con herramientas gratuitas", "hernan.quintero@correo.com", DateTime.Parse("2027-02-20T14:00:00"), DateTime.Parse("2027-02-20T18:00:00"), 12);
        await EnsureWorkshopAsync(context, userManager, "MKT-001", "Marketing en redes sociales", "Estrategia de contenido para pequeños negocios", "paola.vargas@correo.com", DateTime.Parse("2026-12-19T08:00:00"), DateTime.Parse("2026-12-19T12:00:00"), 20);
        await EnsureWorkshopAsync(context, userManager, "MKT-002", "Publicidad en Meta e Instagram", "Configuración y medición de campañas", "paola.vargas@correo.com", DateTime.Parse("2027-01-16T14:00:00"), DateTime.Parse("2027-01-16T18:00:00"), 16);
        await EnsureWorkshopAsync(context, userManager, "MKT-003", "Emprendimiento digital", "Validar y lanzar una idea de negocio en línea", "paola.vargas@correo.com", DateTime.Parse("2027-02-27T09:00:00"), DateTime.Parse("2027-02-27T12:00:00"), 20);
    }

    private static async Task EnsureUserAsync(UserManager<ApplicationUser> userManager, string email, string password, string fullName, string role, string? specialty)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                Specialty = specialty,
                Role = role,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }

    private static async Task EnsureWorkshopAsync(SkillHubeduardoDbContext context, UserManager<ApplicationUser> userManager, string code, string title, string description, string instructorEmail, DateTime start, DateTime end, int capacity)
    {
        if (!await context.Workshops.AnyAsync(w => w.Code == code))
        {
            var instructor = await userManager.FindByEmailAsync(instructorEmail);
            if (instructor != null)
            {
                var workshop = new Workshop
                {
                    Code = code,
                    Title = title,
                    Description = description,
                    InstructorId = instructor.Id,
                    StartDate = start,
                    EndDate = end,
                    MaxCapacity = capacity,
                    CreatedAt = DateTime.UtcNow
                };
                await context.Workshops.AddAsync(workshop);
                await context.SaveChangesAsync();
            }
        }
    }
}