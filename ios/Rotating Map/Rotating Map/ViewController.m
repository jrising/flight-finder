//
//  ViewController.m
//  Rotating Map
//
//  Created by James Rising on 11/17/13.
//  Copyright (c) 2013 Swift Concepts. All rights reserved.
//

#import "ViewController.h"
#import "AnimationManager.h"
#import <math.h>

@interface ViewController () {
    AnimationManager* animator;

    UIImage* image;
    UIImageView* rightView;
    UIImageView* leftView;
    CGFloat rotation;
}
@end

@implementation ViewController

- (void)viewDidLoad
{
    [super viewDidLoad];

    // Create the image from a png file
    image = [UIImage imageNamed:@"nightlights.png"];
    
    leftView = [[UIImageView alloc] initWithImage:image];
    leftView.frame = CGRectMake(0, 0, image.size.width * self.view.frame.size.height / image.size.height, self.view.frame.size.height);
    [self.view addSubview:leftView];

    rightView = [[UIImageView alloc] initWithImage:image];
    rightView.frame = CGRectMake(leftView.frame.size.width, 0, leftView.frame.size.width, leftView.frame.size.height);
    [self.view addSubview:rightView];

    rotation = 0;
    [self rotateMap:0];

    animator = [[AnimationManager alloc] init];
    [animator startAnimation:nil target:self selector:@selector(rotateMap:)];
}

- (void)viewDidUnload
{
    [super viewDidUnload];
}

-(bool) rotateMap:(NSTimeInterval)elapsedTime {
    rotation += elapsedTime / 60.0;
    
    CGFloat scaledWidth = image.size.width * self.view.frame.size.height / image.size.height;
    CGFloat leftOrigin = -fmodf(rotation, 1.0) * scaledWidth;
    
    leftView.frame = CGRectMake(leftOrigin, 0, scaledWidth, self.view.frame.size.height);
    rightView.frame = CGRectMake(leftView.frame.origin.x + leftView.frame.size.width, 0, leftView.frame.size.width, leftView.frame.size.height);

    return true;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        return (interfaceOrientation != UIInterfaceOrientationPortraitUpsideDown);
    } else {
        return YES;
    }
}

@end
