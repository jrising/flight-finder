/******************************************************************\
 *     File Name: AnimationManager.h
 *      -----------------------------------------------------------
 * Helper for constructing arbitrary frame-update animations
\******************************************************************/

#import <Foundation/Foundation.h>
#import <QuartzCore/QuartzCore.h>

@interface AnimationManager : NSObject {
    CADisplayLink* displayLink;
    CFTimeInterval lastDrawTime;
    
    UIView* view;
    id target;
    SEL selector;
    id userInfo;
    
    NSMutableArray* onCompletes;
}

-(id) init;

-(void) startAnimation:(UIView*)view target:(id)target selector:(SEL)selector;
-(void) startAnimation:(UIView*)view target:(id)target selector:(SEL)selector userInfo:(id)userInfo;
-(void) completeAnimation;

-(void) callTarget:(id)target onComplete:(SEL)selector;

+(AnimationManager*) startAnimation:(UIView*)view target:(id)target selector:(SEL)selector userInfo:(id)userInfo;

@end
