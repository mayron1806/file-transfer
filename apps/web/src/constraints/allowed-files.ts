export const allowedImages = [
  ".jpg", ".jpeg", ".png", ".gif", ".svg", ".bmp", ".webp", ".tiff", ".ico",
]
export const allowedDocuments = [
  ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", ".odt", ".ods", ".odp", ".rtf", ".md",
]
export const allowedAudio = [
  ".mp3", ".wav", ".ogg", ".flac", ".aac", ".m4a",
]
export const allowedVideo = [
  ".mp4", ".mov", ".avi", ".mkv", ".flv", ".wmv", ".webm",
];
export const allowedCompressedFiles = [
  ".zip", ".gz", ".rar", ".tar", ".7z", ".bz2", ".xz",
]
export const allowedOthers = [
  ".csv", ".xml", ".json"
]
export const allowedFiles = [
  ...allowedImages,
  ...allowedDocuments,
  ...allowedAudio,
  ...allowedVideo,
  ...allowedCompressedFiles,
  ...allowedOthers
]