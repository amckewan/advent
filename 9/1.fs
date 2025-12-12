\ Day 9 , part 1

include ../init.fs

true constant example

2 cells constant #tile

create tiles
example [if]
( 0 ) 7 , 1 ,
( 1 ) 11 , 1 ,
( 2 ) 11 , 7 ,
( 3 ) 9 , 7 ,
( 4 ) 9 , 5 ,
( 5 ) 2 , 5 ,
( 6 ) 2 , 3 ,
( 7 ) 7 , 3 ,
[else]
include input.fs
[then]

here tiles - constant size

: area ( x1 y1 x2 y2 -- n )
    rot - abs 1+  -rot - abs 1+  * ;

0 value largest
: part1 ( -- n )  0 to largest
    tiles size bounds do
        tiles size bounds do
            i 2@ j 2@ area  dup largest > if  dup to largest  then drop
        #tile +loop
    #tile +loop largest ;

example [if]
T{ part1 -> 50 }T
[else]
T{ part1 -> 4771532800 }T
[then]
