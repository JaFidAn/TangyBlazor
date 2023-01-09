using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models.DTOs;

namespace Tangy_Business.Repository
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductPriceRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductPriceDTO> Create(ProductPriceDTO objDTO)
        {
            //ProductPrice ProductPrice = new ProductPrice()
            //{
            //    Id = objDTO.Id,
            //    Name = objDTO.Name,
            //    CreatedDate = DateTime.Now,
            //};
            ProductPrice productPrice = _mapper.Map<ProductPriceDTO, ProductPrice>(objDTO);

            _db.ProductPrices.Add(productPrice);
            await _db.SaveChangesAsync();

            //return new ProductPriceDTO()
            //{
            //    Id = ProductPrice.Id,
            //    Name = ProductPrice.Name,
            //};
            return _mapper.Map<ProductPrice, ProductPriceDTO>(productPrice);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.ProductPrices.FirstOrDefaultAsync(c => c.Id == id);
            if(obj!=null)
            {
                _db.ProductPrices.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductPriceDTO> Get(int id)
        {
            var obj = await _db.ProductPrices.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null)
            {
               return _mapper.Map<ProductPrice, ProductPriceDTO>(obj);
            }
            return new ProductPriceDTO();
        }

        public async Task<IEnumerable<ProductPriceDTO>> GetAll(int? id=null)
        {
            if(id!=null&& id > 0)
            {
				return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(_db.ProductPrices.Where(u=>u.ProductId==id));
			}
            else
            {
				return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(_db.ProductPrices);
			}
        }

        public async Task<ProductPriceDTO> Update(ProductPriceDTO objDTO)
        {
            var objFromDb = await _db.ProductPrices.FirstOrDefaultAsync(c => c.Id == objDTO.Id);
            if(objFromDb!=null)
            {
                objFromDb.Price= objDTO.Price;
				objFromDb.Size = objDTO.Size;
				_db.ProductPrices.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<ProductPrice, ProductPriceDTO>(objFromDb);
            }
            return objDTO;
        }
    }
}
