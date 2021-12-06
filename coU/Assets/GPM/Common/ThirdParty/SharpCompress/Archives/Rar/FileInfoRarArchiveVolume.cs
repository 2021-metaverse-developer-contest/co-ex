#if CSHARP_7_3_OR_NEWER

#if !NO_FILE
using System.Collections.Generic;
using System.IO;
using Gpm.Common.ThirdParty.SharpCompress.Common.Rar;
using Gpm.Common.ThirdParty.SharpCompress.Common.Rar.Headers;
using Gpm.Common.ThirdParty.SharpCompress.IO;
using Gpm.Common.ThirdParty.SharpCompress.Readers;

namespace Gpm.Common.ThirdParty.SharpCompress.Archives.Rar
{
    /// <summary>
    /// A rar part based on a FileInfo object
    /// </summary>
    internal class FileInfoRarArchiveVolume : RarVolume
    {
        internal FileInfoRarArchiveVolume(FileInfo fileInfo, ReaderOptions options)
            : base(StreamingMode.Seekable, fileInfo.OpenRead(), FixOptions(options))
        {
            FileInfo = fileInfo;
            FileParts = GetVolumeFileParts().ToReadOnly();
        }

        private static ReaderOptions FixOptions(ReaderOptions options)
        {
            //make sure we're closing streams with fileinfo
            options.LeaveStreamOpen = false;
            return options;
        }

        internal ReadOnlyCollection<RarFilePart> FileParts { get; }

        internal FileInfo FileInfo { get; }

        internal override RarFilePart CreateFilePart(MarkHeader markHeader, FileHeader fileHeader)
        {
            return new FileInfoRarFilePart(this, ReaderOptions.Password, markHeader, fileHeader, FileInfo);
        }

        internal override IEnumerable<RarFilePart> ReadFileParts()
        {
            return FileParts;
        }
    }
}
#endif

#endif