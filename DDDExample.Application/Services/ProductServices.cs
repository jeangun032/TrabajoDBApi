using AutoMapper;
using DDDExample.Application.DTOs;
using DDDExample.Domain.Entities;
using DDDExample.Domain.Repositories;

namespace DDDExample.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(data);
        }

        public async Task<ProductDto> AddAsync(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            await _repo.AddAsync(product);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
