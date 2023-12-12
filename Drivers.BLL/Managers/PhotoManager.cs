using AutoMapper;
using Drivers.BLL.DTOs.Requests;
using Drivers.BLL.Managers.Contracts;
using Drivers.DAL.ADO.UOW.Contracts;
using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.UOW.Contracts;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Drivers.BLL.Managers
{
    public class PhotoManager : IPhotoManager
    {
        private readonly ILogger<PhotoManager> _logger;
        private readonly IMapper _mapper;
        private IUnitOfWork _ADOuow;
        private IEFUnitOfWork _EFuow;

        public PhotoManager(ILogger<PhotoManager> logger,
            IMapper mapper,
            IUnitOfWork ado_unitofwork,
            IEFUnitOfWork eFUnitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _ADOuow = ado_unitofwork;
            _EFuow = eFUnitOfWork;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /// IMAGESHARP
        /////////////////////////////////////////////////////////////////////////////////////////////
        public async Task ImageSharp_CompressAndSaveImageAsync(IFormFile imageFile,
            string outputPath,
            int quality = 50)
        {
            using var imageStream = imageFile.OpenReadStream();
            using var image = Image.Load(imageStream);

            //Transformation -> resize / gray
            image.Mutate(x => x
                 .Resize(image.Width / 2, image.Height / 2)
                 .Grayscale());

            // Transormation -> Compression
            var encoder = new JpegEncoder
            {
                Quality = quality,
            };

            await Task.Run(() => image.Save(outputPath, encoder));
        }


        /////////////////////////////////////////////////////////////////////////////////////////////
        /// MagicImage
        /////////////////////////////////////////////////////////////////////////////////////////////
        public async Task MagickNet_CompressAndResizeImage(IFormFile imageFile,
            string outputImagePath,
            int quality = 50,
            int width = 100,
            int height = 100)
        {
            using (var imageStream = imageFile.OpenReadStream())
            using (MagickImage image = new MagickImage(imageStream))
            {
                // Resize the image to the requested width and height
                image.Resize(width, height);

                // Set the compression quality
                image.Quality = quality;

                // Save the modified image to the output path
                using (var outputStream = new FileStream(outputImagePath, FileMode.Create))
                {
                    await image.WriteAsync(outputStream);
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /// Simple upload
        /////////////////////////////////////////////////////////////////////////////////////////////
        public async Task UploadImage(IFormFile imageFile)
        {
            //todo
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /// Database upload
        /////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<EFPhoto> WriteToDbAsync(IFormFile image, CancellationToken cancellationToken)
        {
            try
            {
                await _EFuow.BeginTransactionAsync(cancellationToken);

                var photoReqDTO = new PhotoReqDTO();

                using (var memoryStream = new MemoryStream())
                {
                    //await image.CopyToAsync(memoryStream);


                    using var imageStream = image.OpenReadStream();

                    var originalImage = Image.Load(imageStream);

                    // Resize the image to 100x100 pixels
                    originalImage.Mutate(x => x
                        .Resize(new ResizeOptions
                        {
                            Size = new Size(100, 100),
                            Mode = ResizeMode.Max
                        }));

                    // Convert the resized image to bytes
                    using (var resizedImageStream = new MemoryStream())
                    {
                        originalImage.Save(resizedImageStream, new JpegEncoder());
                        var resizedImageData = resizedImageStream.ToArray();

                        photoReqDTO.PhotoData = resizedImageData;
                    }

                    photoReqDTO.ContentType = image.ContentType;
                    photoReqDTO.FileName = image.FileName;
                    photoReqDTO.FileSize = photoReqDTO.PhotoData.Length;
                }

                var result = await _EFuow.EFPhotoRepository.AddAsync(_mapper.Map<EFPhoto>(photoReqDTO));
                await _EFuow.CompleteAsync(cancellationToken);
                await _EFuow.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                await _EFuow.RollbackTransactionAsync(cancellationToken);
                throw new ApplicationException("Error in transaction while adding information about driver", ex);
            }
        }
    }
}
