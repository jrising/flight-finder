/******************************************************************\
 *     File Name: AnimationManager.m
 *     Owned By:  James R
 *     Copyright: Liiiike Shopping Inc.
 *      -----------------------------------------------------------
 * Registers a callback for each frame, and passes that on to
 * an arbitrary selector
\******************************************************************/

#import "AnimationManager.h"

@interface AnimationManager () {
    NSInvocation* invoke;
}

@end

@implementation AnimationManager

-(id) init {
    if (self = [super init]) {
        displayLink = nil;
        lastDrawTime = 0;
        onCompletes = [[NSMutableArray alloc] init];
    }
    
    return self;
}

-(void) startAnimation:(UIView*)_view target:(id)_target selector:(SEL)_selector {
    [self completeAnimation];

    NSMethodSignature* signature = [AnimationManager instanceMethodSignatureForSelector:@selector(dummySelector:)];
    invoke = [NSInvocation invocationWithMethodSignature:signature];

    view = _view;
    target = _target;
    selector = _selector;
    lastDrawTime = 0;
    userInfo = nil;
    
    [invoke setTarget:_target];
    [invoke setSelector:_selector];
        
    displayLink = [CADisplayLink displayLinkWithTarget:self selector:@selector(animationStep)];

    [displayLink addToRunLoop:[NSRunLoop mainRunLoop] forMode:NSDefaultRunLoopMode];
}

-(void) startAnimation:(UIView*)_view target:(id)_target selector:(SEL)_selector userInfo:(id)_userInfo {
    [self completeAnimation];

    NSMethodSignature* signature = [AnimationManager instanceMethodSignatureForSelector:@selector(dummySelector:userInfo:)];
    invoke = [NSInvocation invocationWithMethodSignature:signature];
    
    view = _view;
    target = _target;
    selector = _selector;
    userInfo = _userInfo;
    lastDrawTime = 0;
    
    [invoke setTarget:_target];
    [invoke setSelector:_selector];
    /*CFTimeInterval zero = 0;
     [invoke setArgument:&zero atIndex:2];*/
    [invoke setArgument:&userInfo atIndex:3];
    
    displayLink = [CADisplayLink displayLinkWithTarget:self selector:@selector(animationStep)];
    
    [displayLink addToRunLoop:[NSRunLoop mainRunLoop] forMode:NSDefaultRunLoopMode];
}

-(void) completeAnimation {
    if (displayLink) {
        [displayLink invalidate];
        displayLink = nil;
    }

    NSArray* occopy = onCompletes.copy;
    [onCompletes removeAllObjects];

    for (NSInvocation* onComplete in occopy)
        [onComplete invoke];
}

-(void) animationStep {
    if (lastDrawTime == 0) {
        lastDrawTime = displayLink.timestamp;
        return;
    }
    
    CFTimeInterval elapsedTime = displayLink.timestamp - lastDrawTime;
    lastDrawTime = displayLink.timestamp;
    
    [invoke setArgument:&elapsedTime atIndex:2];
        
    [invoke invoke];
    bool continues;
    [invoke getReturnValue:&continues];
    
    if (continues) {
        if (view)
            [view setNeedsDisplay];
    } else
        [self completeAnimation];
}

-(bool) dummySelector:(NSTimeInterval)elapsedTime {
    return true;
}

-(bool) dummySelector:(NSTimeInterval)elapsedTime userInfo:(id)userInfo {
    return true;
}

-(void) callTarget:(id)_target onComplete:(SEL)_selector {
    NSInvocation* onComplete = [NSInvocation invocationWithMethodSignature:[target methodSignatureForSelector:_selector]];
    onComplete.target = _target;
    onComplete.selector = _selector;
    
    [onCompletes addObject:onComplete];
}

+(AnimationManager*) startAnimation:(UIView*)view target:(id)target selector:(SEL)selector userInfo:(id)userInfo {
    AnimationManager* manager = [[AnimationManager alloc] init];
    [manager startAnimation:view target:target selector:selector userInfo:userInfo];
    
    return manager;
}
                            
@end
