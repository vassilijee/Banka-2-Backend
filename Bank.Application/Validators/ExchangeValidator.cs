﻿using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.Application.Utilities;

using FluentValidation;

namespace Bank.Application.Validators;

public static class ExchangeValidator
{
    public class ExchangeBetween : AbstractValidator<ExchangeBetweenQuery>
    {
        public ExchangeBetween()
        {
            RuleFor(request => request.CurrencyFromCode)
            .NotEmpty()
            .WithMessage(ValidationErrorMessage.Currency.CodeEmpty)
            .Length(3)
            .WithMessage(ValidationErrorMessage.Currency.CodeLenght)
            .Must(ValidatorUtilities.Global.ContainsOnlyLetters)
            .WithMessage(ValidationErrorMessage.Currency.CodeInvalid);

            RuleFor(request => request.CurrencyToCode)
            .NotEmpty()
            .WithMessage(ValidationErrorMessage.Currency.CodeEmpty)
            .Length(3)
            .WithMessage(ValidationErrorMessage.Currency.CodeLenght)
            .Must(ValidatorUtilities.Global.ContainsOnlyLetters)
            .WithMessage(ValidationErrorMessage.Currency.CodeInvalid);
        }
    }

    public class Update : AbstractValidator<ExchangeUpdateRequest>
    {
        public Update() { }
    }

    public class MakeExchange : AbstractValidator<ExchangeMakeExchangeRequest>
    {
        public MakeExchange()
        {
            RuleFor(request => request.CurrencyFromId)
            .NotEmpty()
            .WithMessage(ValidationErrorMessage.Currency.IdEmpty)
            .NotNull()
            .WithMessage(ValidationErrorMessage.Currency.IdNull);

            RuleFor(request => request.CurrencyToId)
            .NotEmpty()
            .WithMessage(ValidationErrorMessage.Currency.IdEmpty)
            .NotNull()
            .WithMessage(ValidationErrorMessage.Currency.IdNull);

            RuleFor(request => request.AccountId)
            .NotEmpty()
            .WithMessage(ValidationErrorMessage.Currency.IdEmpty)
            .NotNull()
            .WithMessage(ValidationErrorMessage.Currency.IdNull);

            RuleFor(request => request.Amount)
            .GreaterThan(0)
            .WithMessage(ValidationErrorMessage.Global.AmountInvalid);
        }
    }
}
