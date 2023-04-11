using PharmacyEdition.Domain.Entities;
using PharmacyEdition.Domain.Enums;
using PharmacyEdition.Models;
using PharmacyEdition.Service.DTOs;
using PharmacyEdition.Service.Helpers;
using PharmacyEdition.Service.Interfaces;
using PharmacyEditon.Data.IRepositories;
using PharmacyEditon.Data.Repositories;

namespace PharmacyEdition.Service.Services;

public class PaymentService : IPaymentService
{
    private IPaymentRepository paymentRepository = new PaymentRepository();
    private ICreditCardService creditCardService = new CreditCardService();
    private readonly IPaymentRepository paymentRepository = new PaymentRepository();

    public PaymentService(IPaymentRepository paymentRepository)
    {
        this.paymentRepository = paymentRepository;
    }
    public PaymentService()
    {
        
    }
    public async ValueTask<Response<Payment>> AddAsync(PaymentCreationDto model)
    {
        long? creditCardId;

        if (model.Type != PaymentType.Cash)
        {
            var creditCard = (await creditCardService
                .GetByIdAsync(model.CreditCardId is null ? -1 : (long)model.CreditCardId)).Value;

            if (creditCard is null)
            {
                return new Response<Payment>
                {
                    StatusCode = 400,
                    Message = "Could not find the credit card"
                };
            }

            creditCardId = creditCard.Id;
        }
        else
        {
            creditCardId = null;
        }

        var mappedEntity = new Payment
        {
            CreatedAt = DateTime.UtcNow,
            CreditCardId = creditCardId,
            IsPaid = model.IsPaid,
            Type = model.Type
        };

        var insertedEntity = await paymentRepository.InsertAsync(mappedEntity);

        return new Response<Payment>
        {
            StatusCode = 200,
            Message = "Success",
            Value = insertedEntity
        };
    }

    public async ValueTask<Response<bool>> DeleteAsync(long id)
    {
        var existingEntity = paymentRepository.SelectAsync(u => u.Id == id);

        if (existingEntity is null)
            return new Response<bool>
            {
                StatusCode = 404,
                Message = "Not found",
                Value = false
            };

        await paymentRepository.DeleteAsync(u => u.Id == id);

        return new Response<bool>
        {
            StatusCode = 200,
            Message = "Success",
            Value = true
        };
    }

    public async ValueTask<Response<List<Payment>>> GetAllAsync()
    {
        var entities = paymentRepository.SelectAllAsync().ToList();

        return new Response<List<Payment>>
        {
            StatusCode = 200,
            Message = "Success",
            Value = entities
        };
    }

    public async ValueTask<Response<Payment>> GetByIdAsync(long id)
    {
        var entity = paymentRepository.SelectAsync(u => u.Id == id);

        if (entity is null)
            return new Response<Payment>
            {
                StatusCode = 404,
                Message = "Not found"
            };

        return new Response<Payment>
        {
            StatusCode = 200,
            Message = "Success",
            Value = entity
        };
    }

    public async ValueTask<Response<Payment>> UpdateAsync(long id, PaymentCreationDto model)
    {
        var existedEntity = paymentRepository.SelectAsync(u => u.Id == id);
        if (existedEntity is null)
            return new Response<Payment>
            {
                StatusCode = 404,
                Message = "Not found"
            };

        if (model.Type != PaymentType.Cash)
        {
            var creditCard = (await creditCardService
                .GetByIdAsync(model.CreditCardId is null ? -1 : (long)model.CreditCardId)).Value;
            
            if (creditCard is null)
            {
                return new Response<Payment>
                {
                    StatusCode = 400,
                    Message = "Could not find the credit card"
                };
            }

            existedEntity.CreditCardId = model.CreditCardId;
        }
        else
        {
            existedEntity.CreditCardId = null;
        }

        existedEntity.UpdatedAt = DateTime.UtcNow;
        existedEntity.IsPaid = model.IsPaid;
        existedEntity.Type = model.Type;

        var updatedEntity = await paymentRepository.UpdateAsync(existedEntity.Id, existedEntity);

        return new Response<Payment>
        {
            StatusCode = 200,
            Message = "Success",
            Value = updatedEntity
        };
    }
}