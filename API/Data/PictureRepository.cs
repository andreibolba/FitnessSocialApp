using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;

namespace API.Data
{
    public class PictureRepository : IPictureRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public PictureRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public PictureDto AddPicture(PictureDto picture)
        {
            var pic = _mapper.Map<Picture>(picture);

            _context.Pictures.Add(pic);

            return SaveAll() ?  _mapper.Map<PictureDto>(pic) : null;
        }

        public PictureDto EditPicture(PictureDto picture)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PictureDto> GetAll()
        {
            return _mapper.Map<IEnumerable<PictureDto>>(_context.Pictures.Where(g => g.Deleted == false));
        }

        public PictureDto GetById(int id)
        {
            return GetAll().FirstOrDefault(g=>g.PictureId == id);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
