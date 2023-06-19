using API.Dtos;
using API.Models;

namespace API.Interfaces.Repository
{
    public interface IPictureRepository
    {
        public IEnumerable<PictureDto> GetAll();
        public PictureDto GetById(int id);
        public PictureDto AddPicture(PictureDto picture);
        public PictureDto EditPicture(PictureDto picture);

        public bool SaveAll();
    }
}
