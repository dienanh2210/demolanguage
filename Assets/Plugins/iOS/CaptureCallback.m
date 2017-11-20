
#import "CaptureCallback.h"
 
@implementation CaptureCallback
 
- (void) savingImageIsFinished:(UIImage *)_image didFinishSavingWithError:(NSError *)_error contextInfo:(void *)_contextInfo
{
    UnitySendMessage("CaptureScreenShot", "DidImageWriteToAlbum", "");
}
 
@end