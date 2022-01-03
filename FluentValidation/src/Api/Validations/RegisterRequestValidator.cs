using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DomainModel;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Validations
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        //public RegisterRequestValidator(StateRepository repository)
        public RegisterRequestValidator()
        {
            // Transform(x => x.Name, x => (x ?? "").Trim()).NotEmpty().Length(0, 200);
            RuleSet("Email", () =>
            {
                RuleFor(x => x.Email)
                .NotEmpty()
                .Length(0, 150)
                .EmailAddress();
            });

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(0, 200)
                .WithMessage("Student Name is needed to register.");

            RuleFor(x => x.Addresses)
                .NotNull()
                .SetValidator(new AddressesValidator());            

            //RuleForEach(x => x.Addresses).NotNull().SetValidator(new AddressValidator());

            //When(x => x.Email == null, () =>
            //{
            //    RuleFor(x => x.Phone).NotEmpty();
            //});
            //When(x => x.Phone == null, () =>
            //{
            //    RuleFor(x => x.Email).NotEmpty();
            //});
        }
    }

    public class EditPersonalInfoRequestValidator : AbstractValidator<EditPersonalInfoRequest>
    {
        public EditPersonalInfoRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(0, 200);
            RuleFor(x => x.Addresses)
                .NotNull()
                .SetValidator(new AddressesValidator());

            //RuleFor(x => x.Addresses).NotNull().SetValidator(new AddressesValidator(repository));
        }
    }

    public class StudentValidator : AbstractValidator<StudentDto>
    {
        public StudentValidator()
        {
            When(x => x.Email is null, () =>
            {
                RuleFor(x => x.Phone)
                    .NotEmpty()
                    .WithMessage("You must provide an email address");
            });
            When(x => x.Phone is null, () =>
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .WithMessage("You must provide a phone number");
                    
            });

            //RuleFor(x => x.Email)
            //    .NotEmpty()
            //    .Length(0, 150)
            //    .EmailAddress()
            //    .When(x => x.Email is not null);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MustBeValueObject(x => Email.Create(x))
                .When(x => x.Email is not null);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .Matches("^[2-9][0-9]{9}$")
                
                .When(x => x.Phone is not null);

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(0, 200)
                .WithMessage("Student Name is needed to register.");

            RuleFor(x => x.Addresses)
                .NotNull()
                .SetValidator(new AddressesValidator());
        }
    }    

    public class AddressValidator : AbstractValidator<AddressDto>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Street).NotEmpty().Length(0, 100);
            RuleFor(x => x.City).NotEmpty().Length(0, 40);
            RuleFor(x => x.State).NotEmpty().Length(0, 2);
            RuleFor(x => x.ZipCode).NotEmpty().Length(0, 5);
        }
    }

    public class AddressesValidator : AbstractValidator<List<AddressDto>>
    {
        public AddressesValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .ListMustContainItems(1, 3)
                //.Must(x => x?.Count >= 1 && x.Count <= 3)
                //.WithMessage("Must have between 1 to 3 addresses")
                .ForEach(x =>
                {
                    x.NotNull();
                    x.SetValidator(new AddressValidator());
                });

            //RuleFor(x => x)
            //    .ListMustContainNumberOfItems(1, 3)
            //    .ForEach(x =>
            //    {
            //        x.NotNull();
            //        x.ChildRules(address =>
            //        {
            //            address.CascadeMode = CascadeMode.Stop;
            //            address.RuleFor(y => y.State)
            //                .MustBeValueObject(s => State.Create(s, repository.GetAll()));
            //            address.RuleFor(y => y)
            //                .MustBeEntity(y => Address.Create(y.Street, y.City, y.State, y.ZipCode, repository.GetAll()));
            //        });
            //    });
        }
    }

    public static class CustomValidators
    {
        public static IRuleBuilderOptionsConditions<T, IList<TElement>> ListMustContainItems<T, TElement>(
            this IRuleBuilder<T, IList<TElement>> ruleBuilder, int? min = null, int? max = null)
        {
            return ruleBuilder.Custom((list, context) =>
            {
                if (min.HasValue && list.Count < min.Value)
                {
                    context.AddFailure(
                        context.PropertyName, 
                        $"The list contains {list.Count} item(s). It must contain at least {min.Value} item(s)"
                    );
                }

                if (max.HasValue && list.Count > max.Value)
                {
                    context.AddFailure(
                        context.PropertyName,
                        $"The list contains {list.Count} item(s). It must contain no more than {max.Value} item(s)"
                    );
                }
            });
        }

        public static IRuleBuilderOptions<T, string> MustBeValueObject<T, TValueObject>(
            this IRuleBuilder<T, string> ruleBuilder,
            Func<string, Result<TValueObject, Error>> factoryMethod)
            where TValueObject : ValueObject
        {
            return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject, Error> result = factoryMethod(value);

                if (result.IsFailure)
                {
                    context.AddFailure(result.Error.Serialize());
                }
            });
        }
        //    public static IRuleBuilderOptions<T, TProperty> NotEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        //    {
        //        return DefaultValidatorExtensions.NotEmpty(ruleBuilder)
        //            .WithMessage(Errors.General.ValueIsRequired().Serialize());
        //    }

        //    public static IRuleBuilderOptions<T, string> Length<T>(this IRuleBuilder<T, string> ruleBuilder, int min, int max)
        //    {
        //        return DefaultValidatorExtensions.Length(ruleBuilder, min, max)
        //            .WithMessage(Errors.General.InvalidLength().Serialize());
        //    }

        //    public static IRuleBuilderOptions<T, TElement> MustBeEntity<T, TElement, TValueObject>(
        //        this IRuleBuilder<T, TElement> ruleBuilder,
        //        Func<TElement, Result<TValueObject, Error>> factoryMethod)
        //        where TValueObject : DomainModel.Entity
        //    {
        //        return (IRuleBuilderOptions<T, TElement>)ruleBuilder.Custom((value, context) =>
        //        {
        //            Result<TValueObject, Error> result = factoryMethod(value);

        //            if (result.IsFailure)
        //            {
        //                context.AddFailure(result.Error.Serialize());
        //            }
        //        });
        //    }

        //    public static IRuleBuilderOptionsConditions<T, IList<TElement>> ListMustContainNumberOfItems<T, TElement>(
        //        this IRuleBuilder<T, IList<TElement>> ruleBuilder, int? min = null, int? max = null)
        //    {
        //        return ruleBuilder.Custom((list, context) =>
        //        {
        //            if (min.HasValue && list.Count < min.Value)
        //            {
        //                context.AddFailure(Errors.General.CollectionIsTooSmall(min.Value, list.Count).Serialize());
        //            }

        //            if (max.HasValue && list.Count > max.Value)
        //            {
        //                context.AddFailure(Errors.General.CollectionIsTooLarge(max.Value, list.Count).Serialize());
        //            }
        //        });
        //    }
    }

    //public class EnrollRequestValidator : AbstractValidator<EnrollRequest>
    //{
    //    public EnrollRequestValidator()
    //    {
    //        RuleFor(x => x.Enrollments)
    //            .NotEmpty()
    //            .ListMustContainNumberOfItems(min: 1)
    //            .ForEach(x =>
    //            {
    //                x.NotNull();
    //                x.ChildRules(enrollment =>
    //                {
    //                    enrollment.RuleFor(y => y.Course).NotEmpty().Length(0, 100);
    //                    enrollment.RuleFor(y => y.Grade).MustBeValueObject(Grade.Create);
    //                });
    //            });
    //    }
    //}
}
