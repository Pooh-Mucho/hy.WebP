# hy.WebP

hy.WebP is a self-contained wrapper of [libwebp](https://developers.google.com/speed/webp), a library for encoding and decoding WebP images.

hy.WebP supports both 32-bit and 64-bit architectures by calling different DLLs (libwebp140_x86.dll or libwebp140_x64.dll) based on the architecture of the running process. The DLLs are included in the built-in resources. You can also provide your own DLLs by putting them in the same directory of hy.WebP assembly.

## Basic Usage

Encode an image file to a lossy WebP image:

```c#
var imageBytes = File.ReadAllBytes("input.png");
var config = WebPCodeC.CreateConfig(85);
var webpBytes = WebPCodeC.Encode(config, imageBytes);
File.WriteAllBytes("output.webp", webpBytes);
```

Encode an image file to a lossless WebP image:

```c#
var imageBytes = File.ReadAllBytes("input.png");
var config = WebPCodeC.CreateConfigLossless();
var webpBytes = WebPCodeC.Encode(config, imageBytes);
File.WriteAllBytes("output.webp", webpBytes);
```

Encode an image to a lossy WebP image:

```c#
var Image sourceImage = ....
var config = WebPCodeC.CreateConfig(85);
var webpBytes = WebPCodeC.Encode(config, sourceImage);
var webpImage = WebPCodeC.Decode(webpBytes);
```

Encode an image to a lossless WebP image:

```c#
var Image sourceImage = ....
var config = WebPCodeC.CreateConfigLossless();
var webpBytes = WebPCodeC.Encode(config, sourceImage);
var webpImage = WebPCodeC.Decode(webpBytes);
```

Encode pixel data to a WebP image:

```c#
var pixels = ImageArgbData.FromImage(image, new Rectangle(0, 0, image.Width, image.Height));
pixels[1, 2] = Color.Red;
var config = WebPCodeC.CreateConfigLossless();
var webpBytes = WebPCodeC.Encode(config, pixels);
```

Decode a WebP file to an image:

```c#
var webpBytes = File.ReadAllBytes("input.webp");
Bitmap image = WebPCodeC.Decode(webpBytes);
```

Decode a WebP file to ARGB32 pixel data:

```c#
var webpBytes = File.ReadAllBytes("input.webp");
ImageArgbData pixels = WebPCodeC.DecodeToArgb(webpBytes);
Console.WriteLine("Width: " + pixels.Width + "Height: " + pixels.Height);
Console.WriteLine("Pixel color at (1,2): " + pixels[1, 2]);
```

Detect the format of an image is WebP or not:

```c#
var webpBytes = File.ReadAllBytes("input.webp");
// Or read first 12 bytes of the file
var isWebP = WebPCodeC.IsWebPFileHeader(webpBytes);
```

## Advanced Encoding

### Customize the encoding parameters for Lossy WebP

```c#
var config = WebPCodeC.CreateConfig(85);
config.Method = 6;
config.Pass = 3;
config.FilterStrength = 60;
config.FilterSharpness = 0;
config.UseSharpYuv = true;
config.MultiThreading = true;
......
```

```c#
var config = WebPCodeC.CreateConfigPreset(85, WebPPreset.Picture);
config.Method = 6;
config.Pass = 3;
config.FilterStrength = 60;
config.FilterSharpness = 0;
config.UseSharpYuv = true;
config.MultiThreading = true;
......
```

Supported encoding parameters for lossy WebP:

| Parameter | Type | Description |
| --- | --- | --- |
| Quality | float | Quality factor, range: 0 (small) .. 100 (big). |
| Method | int | Quality/Speed trade-off, range: 0 (fast) .. 6 (slower-better). |
| SNSStrength | int | Spatial Noise Shaping, range: 0 (off) .. 100 (maximum). |
| FilterStrength | int | Range: 0 (off) .. 100 (strongest). |
| FilterSharpness | int | Range: 0 (off) .. 7 (least sharp). |
| AutoFilter | bool | Auto adjust filter's strength. |
| FilterType | int | Filtering type: 0 = simple, 1 = strong (only used if FilterStrength > 0 or AutoFilter = true) |
| AlphaCompression | bool | Algorithm for encoding the alpha plane (false = none, true = compressed with WebP lossless). |
| AlphaFiltering | int | Predictive filtering method for alpha plane: 0 = none, 1 = fast, 2 = best. |
| AlphaQuality | int | Range: 0 (smallest size) .. 100 (lossless). |
| Pass | int | Number of entropy-analysis passes (in \[1..10\]). |
| MultiThreading | bool | Use multi-threading if available. |
| UseSharpYuv | bool | Use sharp (and slow) RGB->YUV conversion. |
| QMin | int | Minimum acceptable quality factor (0 - 100). |
| QMax | int | Maximum acceptable quality factor (0 - 100). |

### Customize the encoding parameters for Lossless WebP

```c#
var config = WebPCodeC.CreateConfigLossless();
config.ImageHint = WebPImageHint.Picture;
config.Quality = 100;
config.NearLossless = 90;
config.Method = 6;
config.Pass = 1;
......
```

Supported encoding parameters for lossless WebP:

| Parameter | Type | Description |
| --- | --- | --- |
| Quality | float | Quality factor, range: 0 (small) .. 100 (big). For lossless, 0 is the fastest but gives larger files compared to the slowest, but best, 100 |
| NearLossless | int | Near lossless encoding [0 = max loss .. 100 = off (default)]. The smaller the value, the smaller the file size and the lower the quality. |
| Method | int | Quality/Speed trade-off, range: 0 (fast) .. 6 (slower-better). |
| ImageHint | WebPImageHint | Hint for image type (lossless only). |
| Pass | int | Number of entropy-analysis passes (in \[1..10\]). |
| MultiThreading | bool | Use multi-threading if available. |
| UseSharpYuv | bool | Use sharp (and slow) RGB->YUV conversion. |

### Encoding a custom clip rectangle of an image

```c#
var config = WebPCodeC.CreateConfig(85);
var pixels = ImageArgbData.FromImage(image, new Rectangle(10, 10, 100, 100));
var webpBytes = WebPCodeC.Encode(config, pixels);
```

### Calculate PSNR between two images or pixel data

The PSNR is defined as:

```plaintext
20 * log10(max) - 10 * log10(MSE)
```

The MSE is defined as:

```plaintext
The sum over three channels (RGB) of the squared difference divided by width * height * 3
```

```c#
var originImage = Image.FromFile("input.png");
var config = WebPCodeC.CreateConfig(100);
var webpBytes = WebPCodeC.Encode(config, originImage);
var webpImage = WebPCodeC.Decode(webpBytes);
var psnr = WebPCodeC.PSNR(originImage, webpImage);
```

```c#
var srcPixels = ImageArgbData.FromFile("input.png");

for (float quality = 100; quality >= 75; quality--)
{
    var config = WebPCodeC.CreateConfig(quality);
    var webpBytes = WebPCodeC.Encode(config, srcPixels);
    var webpPixels = WebPCodeC.DecodeToArgb(webpBytes);
    var psnr = WebPCodeC.PSNR(srcPixels, webpPixels);
    Console.WriteLine("Quality: " + quality ", PSNR: " + psnr);
}
```
