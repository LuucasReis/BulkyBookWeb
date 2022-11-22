using BulkyBook.DataAcess.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAcess.Data;
using Microsoft.EntityFrameworkCore;
using BulkyBook.Utility;
using BulkyBook.Models;

namespace BulkyBook.DataAcess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception e)
            {
            }

            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "luke@gmail.com",
                    Email = "luke@gmail.com",
                    Name = "Lucas Reis",
                    PhoneNumber = "31996990786",
                    StreetAddress = "Rua Zeus",
                    State = "Manhattan",
                    Company = null
                }, "#Admin123").GetAwaiter().GetResult();
                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(x => x.Email == "luke@gmail.com");

                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            if (!(_db.Products.Any() || _db.Categories.Any() || _db.CompanyUsers.Any() || _db.CoverTypes.Any()))
            {

                CoverType cv1 = new CoverType("Hard Cover");
                CoverType cv2 = new CoverType("Soft Cover");

                Category c1 = new Category("Ficção", 1);
                Category c2 = new Category("Romance", 2);
                Category c3 = new Category("Fantasia", 3);
                Category c4 = new Category("Ação", 4);
                Category c5 = new Category("Auto ajuda", 5);

                Company company = new Company("Luke's Company", "Rua Andromeda", "Ocean", "Canada", "34567834");
                Company company2 = new Company("Meta", "Rua Zeus", "Manhattan", "Olimpo", "01110000");

                Product p1 = new Product("Supernatural", "<p>H&aacute; vinte e dois anos, Sam e Dean Winchester perderam a m&atilde;e por causa de uma for&ccedil;a sobrenatural misteriosa e demon&iacute;aca. Nos anos que se seguiram, seu pai, John, ensinou-lhes sobre o mal paranormal que vive nos cantos escuros e nas estradas secund&aacute;rias dos Estados Unidos... e como mat&aacute;-lo. Sam e Dean v&ecirc;m &agrave; cidade de Nova Iorque para verificar a casa assombrada de um roqueiro local. Mas antes que eles possam descobrir por que uma banshee apaixonada em uma camiseta heavy-metal dos anos 80 est&aacute; se lamentando no quarto, um crime muito mais macabro chama-lhes a aten&ccedil;&atilde;o. N&atilde;o muito longe da casa, dois estudantes universit&aacute;rios foram espancados at&eacute; a morte por um estranho agressor. Um assassinato, que &eacute; bizarro mesmo para os padr&otilde;es de Nova Iorque, &eacute; o mais recente em uma s&eacute;rie de crimes que os irm&atilde;os logo suspeitam serem baseados nos contos horripilantes do lend&aacute;rio escritor Edgar Allan Poe.</p>"
                    , "9788583111030", "Luke", 20, 18, 16, 12, "\\Images\\Products\\f30e8fb6-e82d-4def-9f5a-7aac896b9ea9.jpg", c1, cv2);

                Product p2 = new Product("Percy Jackson e o Ladrão de Raios", "<p>A vida do adolescente Percy Jackson, que est&aacute; sempre pronto para entrar em uma confus&atilde;o, torna-se bem mais complicada quando ele descobre que &eacute; filho do deus grego Poseidon. Em um campo de treinamento para filhos das divindades, Percy aprende a tirar proveito de seus poderes divinos e se preparea para a maior aventura de sua vida.</p>"
                    , "9788599106297", "Rick Riordan", 37, 35, 32, 29, "\\Images\\Products\\899ee173-28d0-451f-a75e-593a0ecfaddb.jpg", c3, cv1);

                Product p3 = new Product("Do Mil ao Milhão", "<div>Do mil ao milh&atilde;o &eacute; o livro do investidor Thiago Nigro.</div>\r\n<p><strong>O livro aborda os 3 pilares para atingir a liberdade financeira e possui diversos ensinamentos para os investidores</strong>. Os 3 pilares para enriquecer s&atilde;o: gastar bem, investir melhor e ganhar mais.</p>"
                    , "9786555110241", "Thiago Nigro", 27, 25, 23, 20, "\\Images\\Products\\62556052-5c90-41e2-96e9-efa6c5807920.jpg", c5, cv2);

                Product p4 = new Product("Mindset", "<p>O livro&nbsp;<strong>mindset a nova psicologia do sucesso</strong>&nbsp;&eacute; a contribui&ccedil;&atilde;o de Dweck para compreendermos o poder de controlar nossa mente. Atrav&eacute;s de uma s&eacute;rie de mecanismos que nos possibilitam criar uma mentalidade a nosso favor para enfrentarmos dificuldades, desde relacionais at&eacute; financeiras.</p>"
                    , "9788554062088", "Carol Dweck", 30, 27, 25, 23, "\\Images\\Products\\4255e898-2bb2-4a93-ace1-1b2948bccfc7.jpg", c5, cv1);



                _db.CoverTypes.AddRange(cv1, cv2);
                _db.Categories.AddRange(c1, c2, c3, c4, c5);
                _db.CompanyUsers.AddRange(company, company2);
                _db.Products.AddRange(p1,p2,p3,p4);
                _db.SaveChanges();
            }
            return;
        }
    }
}
