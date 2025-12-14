\ Day 9, parts 1 & 2
\ Requires `gforth -m11G`

include ../init.fs

\ ============================================================
\ List of red tiles

0 value reds
0 value #reds

: @red ( n -- x y )  [ 2 cells ] literal *  reds +  2@ ;

create example-reds
( 0 )  7 1 , ,
( 1 ) 11 1 , ,
( 2 ) 11 7 , ,
( 3 )  9 7 , ,
( 4 )  9 5 , ,
( 5 )  2 5 , ,
( 6 )  2 3 , ,
( 7 )  7 3 , ,

\ append first tile for wrap (but not in size)
here example-reds - 2 cells /  example-reds 2@ , ,   constant example-#reds

create input-reds
    include input.fs
here input-reds - 2 cells /   input-reds 2@ , ,   constant input-#reds

: example  example-reds to reds  example-#reds to #reds ;
: input    input-reds   to reds  input-#reds   to #reds ;

example

\ ============================================================
\ Part 1

: area ( x1 y1 x2 y2 -- n )
    rot - abs 1+  -rot - abs 1+  * ;

2variable r1
2variable r2

: part1 ( -- n )  0 ( largest )
    #reds 1- 0 do
        #reds i 1+ do
            j @red i @red area  2dup < if
                swap
                ( diag )  i @red r1 2!  j @red r2 2!
            then drop
        loop
    loop ;

T{ example part1 -> 50 }T
T{ input   part1 -> 4771532800 }T

\ ============================================================
\ Create full grid, requires 10 GB

100000 constant #grid

unused #grid dup * < [if] cr .( Need `gforth -m11G` ) bye [then]

create grid  #grid dup *  allot

: clear  grid  #grid dup *  erase ;

: tile ( x y -- a )  #grid * +  grid + ;

: .tile ( x y -- )  tile c@ if '#' else '.' then emit ;

\ ============================================================
\ Display window

2variable origin

: .window ( x y -- )   2dup origin 2!
    cr 6 spaces over .
    40 bounds do
        cr i 5 .r space
        dup 40 bounds do
            i j .tile
        loop
    loop drop ;

: ww  origin 2@ .window ;

: w  ( x y -- )  swap 20 - 0 max  swap 20 - 0 max  .window ;

: d   40 origin       +!  ww ;
: u  -40 origin       +!  ww ;
: r   40 origin cell+ +!  ww ;
: l  -40 origin cell+ +!  ww ;

\ walk through the reds
0 value r#
: n  r# 1+  #reds mod  dup to r#  @red w ;

\ ============================================================
\ draw border

: draw ( a +n step -- a+n*steps )
    -rot  0 do  over +  1 over c!  loop  nip ;

: drawto ( a dest -- dest )
    over - ( distance )
    dup abs #grid < if ( same row )
        dup 0< if ( left) negate -1  else ( right) 1  then
    else
        dup 0< if ( up) negate #grid /  #grid negate
        else    ( down)        #grid /  #grid           then
    then  draw ;

: draw-border
    0 @red tile ( start )
    #reds 1+ 1 do  i @red tile  drawto  loop drop ;

\ ============================================================
\ state
\ 0 = outside
\ 1 = outside wall
\ 2 = inside ( draw here )
\ 3 = inside wall
\ 4 = outside = 0

: fill-row ( y -- )
    0 swap tile ( start )  0 ( edge )  0 ( state )
    rot #grid bounds do  ( edge state -- )
        over i c@ xor if ( hit edge )
            nip i c@ ( edge )  swap 1+ 3 and ( next state )
        then
        dup 2 = if ( draw ) 1 i c!  then
    loop  2drop ;

\  50278 , 1772 , <- first row
\  51504 , 1772 ,
\  48496 , 98321 , <- last row
\  47265 , 98321 ,

: rows ( from to -- )
    1+ swap do  i 1000 mod 0= if cr i . then  i fill-row  loop ;

: fill-input
    2 6 rows ( example )
    1773 98321 rows ( input ) ;

\ ============================================================
\ Part 2

: check ( a +n step -- f )
    -rot 0 do ( step a )
        dup c@ 0= if  2drop false  unloop exit  then
        over + ( step to next tile )
    loop  2drop  true ;

: checkto ( a dest -- f )
    over - ( distance )
    dup abs #grid < if ( same row )
        dup 0< if  negate -1  else  1  then
    else
        dup 0< if  negate #grid /  #grid negate  else  #grid /  #grid  then
    then check ;

: inside { x1 y1 x2 y2 -- f }
    x1 y1 tile  x1 y2 tile  checkto  0= if  false exit  then
    x1 y2 tile  x2 y2 tile  checkto  0= if  false exit  then
    x2 y2 tile  x2 y1 tile  checkto  0= if  false exit  then
    x2 y1 tile  x1 y1 tile  checkto ;

: part2 ( -- n )  0 ( largest )
    #reds 1- 0 do
        i .
        #reds i 1+ do
            j @red i @red area  2dup < if
                j @red i @red inside if
                    swap
                then
            then drop
        loop
    loop ;


cr .( Initializing...)
clear
example draw-border
input draw-border
fill-input

T{ example part2 -> 24 }T
T{ input   part2 -> 1544362560 }T
