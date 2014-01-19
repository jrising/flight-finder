/******************************************************************************\
 * Airport - Represents a possible origin or desintation
 * ----------------------------------------------------------------------------
 * Copyright (C) 2009, 2013  James Rising
 *
 * This file is part of FFlight, which is free software: you can redistribute
 * it and/or modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 *
 * FFlight is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
 * details (license.txt).
 *
 * You should have received a copy of the GNU General Public License along with
 * FFlight.  If not, see <http://www.gnu.org/licenses/>.
 \******************************************************************************/

#import <Foundation/Foundation.h>

@interface Airport : NSObject

+(NSArray*) loadAllAirports:(NSString*)order;

@end
