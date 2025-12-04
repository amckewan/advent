\ Day 4

[DEFINED] EMPTY [IF] EMPTY [THEN] MARKER EMPTY DECIMAL
include ../ttester.fs

create map   here  200 dup *  dup allot erase

0 value rows
0 value cols

: 'row ( r -- a )    1+  cols 1+ 1+ *  map + ;
: at ( r c -- 0/1)   swap 'row + c@ ;

: .row ( n)   cr 0 .r ." : " ;
: .map  rows 0 do i .row  cols 0 do j i at . loop loop ;

: | ( -- ) \ build a row
    bl parse  cols 0= if dup to cols then
    rows 'row  swap bounds do  count '@' = negate  i c!  loop  drop
    rows 1+ to rows ;

0 [if]
\ Example
| ..@@.@@@@.
| @@@.@.@.@@
| @@@@@.@.@@
| @.@@@@..@.
| @@.@@@@.@@
| .@@@@@@@.@
| .@.@.@.@@@
| @.@@@.@@@@
| .@@@@@@@@.
| @.@.@@@.@.
[else]
include input.fs
[then]

\ count neighbors
: +/- ( n -- limit index )  dup 1+ 1+  swap 1- ;
: 3x3 ( r c -- n )  -1 swap ( r n c )
    +/- do  over +/- do  i j at +  loop loop  nip ;
: ok? ( r c -- 0/1 )
    2dup at if  3x3 4 < negate  else  2drop 0 ( no roll)  then ;

: solve ( -- n )
    0  rows 0 do  cols 0 do  j i ok? +  loop loop ;

\  : .3x3  rows 0 do i .row  cols 0 do j i 3x3 . loop loop ;
\  : .solve  rows 0 do i .row  cols 0 do j i ok? . loop loop ;
