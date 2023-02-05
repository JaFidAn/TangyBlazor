using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_DataAccess.ViewModel;
using Tangy_Models.DTOs;

namespace Tangy_Business.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OrderHeaderDTO> CancelOrder(int id)
        {
            var orderHeader = await _db.OrderHeaders.FindAsync(id);
            if (orderHeader == null)
            {
                return new OrderHeaderDTO();
            }
            if(orderHeader.Status == SD.Status_Pending)
            {
                orderHeader.Status = SD.Status_Cancelled;
                await _db.SaveChangesAsync();
            }
            if(orderHeader.Status == SD.Status_Confirmed)
            {
                //refund
                var options = new Stripe.RefundCreateOptions
                {
                    Reason = Stripe.RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new Stripe.RefundService();
                Stripe.Refund refund = service.Create(options);

                orderHeader.Status = SD.Status_Refunded;
                await _db.SaveChangesAsync();
            }
            return _mapper.Map<OrderHeader, OrderHeaderDTO>(orderHeader);
        }

        public async Task<OrderDTO> Create(OrderDTO objDTO)
        {
            try
            {
                var obj = _mapper.Map<OrderDTO, Order>(objDTO);
                _db.OrderHeaders.Add(obj.OrderHeader);
                await _db.SaveChangesAsync();

                foreach (var details in obj.OrderDetails)
                {
                    details.OrderHeaderId = obj.OrderHeader.Id;
                }
                _db.OrderDetails.AddRange(obj.OrderDetails);
                await _db.SaveChangesAsync();

                return new OrderDTO()
                {
                    OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDTO>(obj.OrderHeader),
                    OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDTO>>(obj.OrderDetails).ToList(),
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int id)
        {
            var objHeader = await _db.OrderHeaders.FirstOrDefaultAsync(o => o.Id == id);
            if (objHeader != null)
            {
                IEnumerable<OrderDetail> orderDetail = _db.OrderDetails.Where(o => o.OrderHeaderId == id);
                _db.OrderDetails.RemoveRange(orderDetail);
                _db.OrderHeaders.Remove(objHeader);
                return _db.SaveChanges();
            }
            return 0;
        }

        public async Task<OrderDTO> Get(int id)
        {
            Order order = new Order()
            {
                OrderHeader = _db.OrderHeaders.FirstOrDefault(o => o.Id == id),
                OrderDetails = _db.OrderDetails.Where(o => o.OrderHeaderId == id)
            };
            if (order != null)
            {
                return _mapper.Map<Order, OrderDTO>(order);
            }
            return new OrderDTO();
        }

        public async Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status = null)
        {
            List<Order> orderFromDb = new List<Order>();
            IEnumerable<OrderHeader> orderHeaderList = _db.OrderHeaders;
            IEnumerable<OrderDetail> orderDetailList = _db.OrderDetails;

            foreach (OrderHeader header in orderHeaderList)
            {
                Order order = new Order()
                {
                    OrderHeader = header,
                    OrderDetails = orderDetailList.Where(o => o.OrderHeaderId == header.Id)
                };
                orderFromDb.Add(order);  //this will add all orders by one by to the List<Order>
            }
            //do some filtering #TODO
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orderFromDb);
        }

        public async Task<OrderHeaderDTO> MarkPaymentSuccessful(int id, string paymentIntentId)
        {
            var data = await _db.OrderHeaders.FindAsync(id);
            if (data == null)
            {
                return new OrderHeaderDTO();
            }
            if (data.Status == SD.Status_Pending)
            {
                data.PaymentIntentId= paymentIntentId;
                data.Status = SD.Status_Confirmed;
                await _db.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDTO>(data);
            }
            return new OrderHeaderDTO();
        }

        public async Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO objDTO)
        {
            if (objDTO != null)
            {
                var orderHeaderFromDb = _db.OrderHeaders.FirstOrDefault(h=>h.Id == objDTO.Id);
                orderHeaderFromDb.Name = objDTO.Name;
                orderHeaderFromDb.PhoneNumber = objDTO.PhoneNumber;
                orderHeaderFromDb.Carier = objDTO.Carier;
                orderHeaderFromDb.Tracking = objDTO.Tracking;
                orderHeaderFromDb.StreetAddress = objDTO.StreetAddress;
                orderHeaderFromDb.City = objDTO.City;
                orderHeaderFromDb.State = objDTO.State;
                orderHeaderFromDb.PostalCode = objDTO.PostalCode;
                orderHeaderFromDb.Status = objDTO.Status;
                await _db.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDTO>(orderHeaderFromDb);
            }
            return new OrderHeaderDTO();
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var data = await _db.OrderHeaders.FindAsync(orderId);
            if (data == null)
            {
                return false;
            }
            data.Status = status;
            if (status == SD.Status_Shipped)
            {
                data.ShippingDate = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
