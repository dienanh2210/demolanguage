#import <Foundation/Foundation.h>
#import <AssetsLibrary/AssetsLibrary.h>
#import <AVFoundation/AVFoundation.h>
#import "CaptureCallback.h"

extern "C" void _PlaySystemShutterSound ()
{
    AudioServicesPlaySystemSound(1108);
}

extern "C" void _GetTexture (const void* byte, int length)
{
    NSData *data = [NSData dataWithBytes:byte length:length];
    UIImage *image = [[UIImage alloc]initWithData:data];
    CaptureCallback *callback = [[CaptureCallback alloc]init];
    UIImageWriteToSavedPhotosAlbum(image, callback, @selector(savingImageIsFinished:didFinishSavingWithError:contextInfo:), nil);
}

