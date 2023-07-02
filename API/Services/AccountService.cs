using API.Contracts;
using API.Data;
using API.DTOs.Accounts;
using API.Models;
using API.Repositories;
using API.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace API.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly ITokenHandler _tokenHandler;
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly BookingDbContext _dBContext;
        public AccountService(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IUniversityRepository universityRepository, IEducationRepository educationRepository, ITokenHandler tokenHandler, IAccountRoleRepository accountRoleRepository, IRoleRepository roleRepository, IEmailHandler emailHandler, BookingDbContext dBContext)
        {
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
            _universityRepository = universityRepository;
            _educationRepository = educationRepository;
            _tokenHandler = tokenHandler;
            _accountRoleRepository = accountRoleRepository;
            _roleRepository = roleRepository;
            _emailHandler = emailHandler;
            _dBContext = dBContext;
        }

        public IEnumerable<GetAccountDto>? GetAccount()
        {
            var accounts = _accountRepository.GetAll();
            if (!accounts.Any())
            {
                return null; // No Account  found
            }

            var toDto = accounts.Select(account =>
                                                new GetAccountDto
                                                {
                                                    Guid = account.Guid,
                                                    Password = account.Password,
                                                    IsDeleted = account.IsDeleted,
                                                    IsUsed = account.IsUsed,
                                                    Otp = account.OTP,
                                                    ExpiredTime = account.ExpiredDate
                                                }).ToList();

            return toDto; // Account found
        }

        public GetAccountDto? GetAccount(Guid guid)
        {
            var account = _accountRepository.GetByGuid(guid);
            if (account is null)
            {
                return null; // account not found
            }

            var toDto = new GetAccountDto
            {
                Guid = account.Guid,
                IsDeleted = account.IsDeleted,
                IsUsed = account.IsUsed,
            };

            return toDto; // accounts found
        }

        public GetAccountDto? CreateAccount(NewAccountDto newAccountDto)
        {
            var account = new Account
            {
                Guid = newAccountDto.Guid,
                Password = Hashing.HashPassword(newAccountDto.Password),
                OTP = GenerateOtp(),
                IsUsed = newAccountDto.IsUsed,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdAccount = _accountRepository.Create(account);
            if (createdAccount is null)
            {
                return null; // Account not created
            }

            var toDto = new GetAccountDto
            {
                Guid = createdAccount.Guid,
                IsDeleted = createdAccount.IsDeleted,
                IsUsed = createdAccount.IsUsed,
            };

            return toDto; // Account created
        }

        public int UpdateAccount(UpdateAccountDto updateAccountDto)
        {
            var isExist = _accountRepository.IsExist(updateAccountDto.Guid);
            if (!isExist)
            {
                return -1; // Account not found
            }

            var getAccount = _accountRepository.GetByGuid(updateAccountDto.Guid);

            var account = new Account
            {
                Guid = updateAccountDto.Guid,
                Password = Hashing.HashPassword(updateAccountDto.Password),
                IsUsed = updateAccountDto.IsUsed,
                IsDeleted = updateAccountDto.IsDeleted,
                ModifiedDate = DateTime.Now,
                CreatedDate = getAccount!.CreatedDate
            };

            var isUpdate = _accountRepository.Update(account);
            if (!isUpdate)
            {
                return 0; // Account not updated
            }

            return 1;
        }

        public int DeleteAccount(Guid guid)
        {
            var isExist = _accountRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // Account not found
            }

            var account = _accountRepository.GetByGuid(guid);
            var isDelete = _accountRepository.Delete(account!);
            if (!isDelete)
            {
                return 0; // Account not deleted
            }

            return 1;
        }

        public RegisterAccountDto? Register(RegisterAccountDto registerDto)
        {
            //using var transaction = _dBContext.Database.BeginTransaction();
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return null;
            }

            var employeeService = new EmployeeService(_employeeRepository);
            Employee employee = new Employee
            {
                Guid = new Guid(),
                NIK = employeeService.GenerateNik(),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                BirthDate = registerDto.BirthDate,
                Gender = registerDto.Gender,
                HiringDate = registerDto.HiringDate,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            var createdEmployee = _employeeRepository.Create(employee);
            if (createdEmployee is null)
            {
                return null;
            }

            var createdUniversity = _universityRepository.GetByCodeandName(registerDto.UniversityCode, registerDto.UniversityName);
            if (createdUniversity == null)
            {
                var university = new University
                {
                    Guid = new Guid(),
                    Code = registerDto.UniversityCode,
                    Name = registerDto.UniversityName,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                createdUniversity = _universityRepository.Create(university);
            }

            Education education = new Education
            {
                Guid = employee.Guid,
                Major = registerDto.Major,
                Degree = registerDto.Degree,
                GPA = registerDto.Gpa,
                UniversityGuid = createdUniversity.Guid
            };
            var createdEducation = _educationRepository.Create(education);
            if (createdEducation is null)
            {
                return null;
            }

            Account account = new Account
            {
                Guid = employee.Guid,
                Password = Hashing.HashPassword(registerDto.Password),
            };
            var createdAccount = _accountRepository.Create(account);
            if (createdAccount is null)
            {
                return null;
            }

            var roleUser = _roleRepository.GetByName("user");
            if(roleUser is null)
            {
                return null;
            }

            var accountRole = new AccountRole
            {
                Guid = Guid.NewGuid(),
                AccountGuid = createdAccount.Guid, 
                RoleGuid =  roleUser.Guid
            };
            var createdUserAccountRole = _accountRoleRepository.Create(accountRole);
            if (createdUserAccountRole is null)
            {
                return null;
            }

            var toDto = new RegisterAccountDto
            {
                FirstName = createdEmployee.FirstName,
                LastName = createdEmployee.LastName,
                BirthDate = createdEmployee.BirthDate,
                Gender = createdEmployee.Gender,
                HiringDate = createdEmployee.HiringDate,
                Email = createdEmployee.Email,
                PhoneNumber = createdEmployee.PhoneNumber,
                Password = createdAccount.Password,
                Major = createdEducation.Major,
                Degree = createdEducation.Degree,
                Gpa = createdEducation.GPA,
                UniversityCode = createdUniversity.Code,
                UniversityName = createdUniversity.Name
            };

           // transaction.Commit();
            return toDto;
        }

        public string Login(LoginAccountDto login)
        {
            var emailEmp = _employeeRepository.GetEmail(login.Email);
            if (emailEmp is null)
            {
                return "0";
            }

            var password = _accountRepository.GetByGuid(emailEmp.Guid);
            var isValid = Hashing.ValidatePassword(login.Password, password!.Password);
            if (!isValid)
            {
                return "-1";
            }

            

            try
            {
                var claims = new List<Claim>() {
                new Claim("NIK", emailEmp.NIK),
                new Claim("FullName", $"{emailEmp.FirstName} {emailEmp.LastName}"),
                new Claim("Email", login.Email)
            };

                var getAccountRole = _accountRoleRepository.GetAccountRolesByAccountGuid(emailEmp.Guid);
                var getRoleNameByAccountRole = from ar in getAccountRole
                                               join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                                               select r.Name;

                claims.AddRange(getRoleNameByAccountRole.Select(role => new Claim(ClaimTypes.Role, role)));

                var getToken = _tokenHandler.GenerateToken(claims);
                return getToken;
            }
            catch
            {
                return "-2";
            }
        }

        public int GenerateOtp()
        {

            Random random = new Random();
            int otp = random.Next(100000, 999999);
            return otp;

        }

        public int ForgotPassword(ForgotPasswordDto forgotPassword)
        {
            var employee = _employeeRepository.GetEmail(forgotPassword.Email);
            if (employee is null)
            {
                return 0;
            }

            var toDto = new ForgotPasswordDto
            {
                Email = employee.Email
            };

            var relatedAccount = _accountRepository.GetByGuid(employee.Guid);
            var otp = GenerateOtp();
            var updateAccountDto = new Account
            {
                Guid = relatedAccount.Guid,
                Password = relatedAccount.Password,
                IsDeleted = (bool)relatedAccount.IsDeleted,
                OTP = otp,
                IsUsed = false,
                ExpiredDate = DateTime.Now.AddMinutes(5),
                ModifiedDate = DateTime.Now
            };

            var updateResult = _accountRepository.Update(updateAccountDto);

            _emailHandler.SendEmail(forgotPassword.Email,
            "Forgot Password",
                                $"Your OTP is {otp}");

            return 1;
        }

        public int ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var isExist = _employeeRepository.GetEmail(changePasswordDto.Email);
            if (isExist is null)
            {
                return -1; // Account not found
            }

            var getAccount = _accountRepository.GetByGuid(isExist.Guid);
            if (getAccount.OTP != changePasswordDto.Otp)
            {
                return 0;
            }

            if (getAccount.IsUsed == true)
            {
                return 1;
            }

            if (getAccount.ExpiredDate < DateTime.Now)
            {
                return 2;
            }

            var account = new Account
            {
                Guid = getAccount.Guid,
                IsUsed = getAccount.IsUsed,
                IsDeleted = getAccount.IsDeleted,
                ModifiedDate = DateTime.Now,
                CreatedDate = getAccount!.CreatedDate,
                OTP = getAccount.OTP,
                ExpiredDate = getAccount.ExpiredDate,
                Password = Hashing.HashPassword(changePasswordDto.NewPassword),
            };

            var isUpdate = _accountRepository.Update(account);
            if (!isUpdate)
            {
                return 0; // Account not updated
            }

            return 3;
        }
    }

}



