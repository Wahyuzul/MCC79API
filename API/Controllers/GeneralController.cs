using API.Contracts;
using API.Data;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Controllers
{
    public class GeneralController<TEntity> : ControllerBase
        where TEntity : class
    {
        protected readonly IGeneralRepository<TEntity> _repository;

        public GeneralController(IGeneralRepository<TEntity> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entity = _repository.GetAll();

            if (!entity.Any())
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var entity = _repository.GetByGuid(guid);
            if (entity is null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpPost]
        public IActionResult Create(TEntity entity)
        {
            var isCreated = _repository.Create(entity);
            return Ok(isCreated);
        }

        [HttpPut]
        public IActionResult Update(TEntity entity)
        {
            var isUpdated = _repository.Update(entity);
            if (!isUpdated)
            {
                return NotFound();
            }
            return Ok(isUpdated);
        }

        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            var isDeleted = _repository.Delete(guid);
            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}

