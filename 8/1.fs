\ Day 8, part 1

include ../init.fs

0 VALUE BOXES
0 VALUE #BOXES

: BOX ( box# -- a )  3 CELLS * BOXES + ;
: X   ( box -- x )   @ ;
: Y   ( box -- y )   CELL + @ ;
: Z   ( box -- z )   2 CELLS + @ ;

: .BOX ( box# )  BOX  DUP X 0 .R  ." ,"  DUP Y 0 .R  ." ,"  Z . ;

: DIST ( b1 b2 -- n ) \ sqrt((x2-x1)^2 + (y2-y1)^2 + (z2-z1)^2)
    OVER BOX X  OVER BOX X -  DUP *  >R
    OVER BOX Y  OVER BOX Y -  DUP *  >R
         BOX Z  SWAP BOX Z -  DUP *  2R> + +
    S>F FSQRT F>S ;

0 VALUE DISTANCES
: 'DIST ( b1 b2 -- a )  #BOXES * +  CELLS DISTANCES + ;
: CALC ( -- ) \ pre-calculate distances
    HERE TO DISTANCES  #BOXES DUP * CELLS ALLOT
    #BOXES 0 DO  #BOXES 0 DO  I J DIST  I J 'DIST !  LOOP LOOP ;
: DISTANCE  ( b1 b2 -- n )  'DIST @ ;

: .DISTANCES
    #BOXES 0 DO  CR #BOXES 0 DO  I J DISTANCE 5 .R  LOOP LOOP ;

: :BOXES  ALIGN  HERE TO BOXES ;
: ;BOXES  HERE BOXES - 3 CELLS / TO #BOXES  CALC ;

: EXAMPLE
    :BOXES
    162  ,  817  ,  812 , 
    57  ,  618  ,  57 , 
    906  ,  360  ,  560 , 
    592  ,  479  ,  940 , 
    352  ,  342  ,  300 , 
    466  ,  668  ,  158 , 
    542  ,  29  ,  236 , 
    431  ,  825  ,  988 , 
    739 , 650 , 466 , 
    52 , 470 , 668 , 
    216 , 146 , 977 , 
    819 , 987 , 18 , 
    117 , 168 , 530 , 
    805 , 96 , 715 , 
    346 , 949 , 466 , 
    970 , 615 , 88 , 
    941 , 993 , 340 , 
    862 , 61 , 35 , 
    984 , 92 , 344 , 
    425 , 690 , 689 , 
    ;BOXES ;
EXAMPLE
T{ 1 3 DISTANCE -> 1 3 DIST }T

: INPUT  :BOXES  S" input.fs" INCLUDED  ;BOXES ;

: ZEROS  0  #BOXES 0 DO  #BOXES 0 DO  J I DISTANCE 0= IF 1+ THEN  LOOP LOOP ;

: SHORTEST ( -- b1 b2 )   0 0 -1
    #BOXES 0 DO  #BOXES 0 DO
      I J DISTANCE  ?DUP IF  OVER U< ( shorter? )
        IF  DROP 2DROP  I J 2DUP DISTANCE  THEN THEN
    LOOP LOOP DROP ;

: CONNECT ( b1 b2 -- )
;
