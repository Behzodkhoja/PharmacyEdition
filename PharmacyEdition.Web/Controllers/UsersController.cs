using Microsoft.AspNetCore.Mvc;
using PharmacyEdition.Service.DTOs;
using PharmacyEdition.Service.Interfaces;
using PharmacyEdition.Service.Services;
using PharmacyEdition.Web.Models;

namespace PharmacyEdition.Web.Controllers;

public class UsersController : Controller
{
    private readonly IUserService userService = new UserService();
    private readonly IOrderService orderService = new OrderService();
    private readonly IPaymentService paymentService = new PaymentService();
    public readonly ICreditCardService creditCardService = new CreditCardService();


    public async Task<IActionResult> Index()
    {
        var users = await this.userService.GetAllAsync();
        return View(users.Value);
    }

    public async Task<IActionResult> Create(UserCreationDto model)
    {
        //var orderss = new List<OrderCreationDto>
        //{
        //    new OrderCreationDto
        //    {
        //        UserId = 2,
        //        Payment = new PaymentCreationDto
        //        {
        //            Type = Domain.Enums.PaymentType.Humo,
        //            IsPaid = true,
        //        },
        //        OrderItems = new List<OrderItemCreationDto>
        //        {
        //            new OrderItemCreationDto
        //            {
        //                MedicineId = 1,
        //                Count = 3
        //            }
        //        }
        //    },
        //    new OrderCreationDto
        //    {
        //        UserId = 2,
        //        Payment = new PaymentCreationDto
        //        {
        //            Type = Domain.Enums.PaymentType.Uzcard,
        //            IsPaid = true,
        //        },
        //        OrderItems = new List<OrderItemCreationDto>
        //        {
        //            new OrderItemCreationDto
        //            {
        //                MedicineId = 2,
        //                Count = 4
        //            },
        //            new OrderItemCreationDto
        //            {
        //                MedicineId = 1,
        //                Count = 5
        //            }
        //        }
        //    },
        //    new OrderCreationDto
        //    {
        //        UserId = 3,
        //        Payment = new PaymentCreationDto
        //        {
        //            Type = Domain.Enums.PaymentType.Humo,
        //            IsPaid = true,
        //        },
        //        OrderItems = new List<OrderItemCreationDto>
        //        {
        //            new OrderItemCreationDto
        //            {
        //                MedicineId = 1,
        //                Count = 6
        //            },
        //            new OrderItemCreationDto
        //            {
        //                MedicineId = 2,
        //                Count = 2
        //            }
        //        }
        //    },
        //    new OrderCreationDto
        //    {
        //        UserId = 4,
        //        Payment = new PaymentCreationDto
        //        {
        //            Type = Domain.Enums.PaymentType.Humo,
        //            IsPaid = true,
        //        },
        //        OrderItems = new List<OrderItemCreationDto>
        //        {
        //            new OrderItemCreationDto
        //            {
        //                MedicineId = 1,
        //                Count = 6
        //            },
        //            new OrderItemCreationDto
        //            {
        //                MedicineId = 2,
        //                Count = 2
        //            }
        //        }
        //    },
        //    new OrderCreationDto
        //    {
        //        UserId = 5,
        //        Payment = new PaymentCreationDto
        //        {
        //            Type = Domain.Enums.PaymentType.Humo,
        //            IsPaid = true,
        //        },
        //        OrderItems = new List<OrderItemCreationDto>
        //        {
        //            new OrderItemCreationDto
        //            {
        //                MedicineId = 2,
        //                Count = 2
        //            }
        //        }
        //    }
        //};
        //foreach (var item in orderss)
        //    await orderService.AddAsync(item);

        var createdUser = await this.userService.AddAsync(model);
        return View(createdUser.Value);
    }

    public async Task<IActionResult> GetOrdersInfo()
    {


        var orders = await this.orderService.GetAllAsync();

        var result = new OrderViewModel();
        var resultList = new List<OrderViewModel>();
        foreach (var order in orders.Value)
        {
            result.Status = order.Status;
            result.User = order.User;
            result.Id = order.Id;
            result.PaymentType = order.Payment.Type;
            resultList.Add(result);
        }
        return View(resultList);
    }
}

