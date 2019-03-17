using Shop2.Data.Infrastructure;
using Shop2.Data.Repositories;
using Shop2.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Service
{
    public interface IOrderService
    {
        // truyền vào ref để trả về chính order đó
        Order Create(ref Order order,List<OrderDetail> orderDetails);
        void UpdateStatus(int orderId);
        void Save();
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository,IOrderDetailRepository orderDetailRepository,IUnitOfWork unitOfWork)
        {
            this._orderDetailRepository = orderDetailRepository;
            this._orderRepository = orderRepository;
            this._unitOfWork = unitOfWork;
        }
        public Order Create(ref Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                // add order(chứa thông tin người mua hàng) vào db
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                // add orderDetail(chứ số lượng sp,sp) vào db
                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }

                return order;
            }
            catch(Exception)
            {
                throw;
            }
           

        }
        
        public void UpdateStatus(int orderId)
        {// update thông tin khi thanh toán thành công
            var order = _orderRepository.GetSingleById(orderId);
            order.Status = true;
            _orderRepository.Update(order);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
