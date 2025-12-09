\ Day 7, Part 2

include ../init.fs

0 VALUE START
0 VALUE ROWS
0 VALUE COLS

: ROW ( n -- a )  COLS * START + ;
: AT ( r c -- c )  SWAP ROW + C@ ;

: D   ROWS 0 DO  CR I ROW COLS TYPE  LOOP ;

: DIAGRAM   ALIGN  HERE TO START  0 TO ROWS  0 TO COLS ;
: |         BL PARSE  DUP TO COLS  HERE SWAP DUP ALLOT CMOVE
            ROWS 1+ TO ROWS ;

DIAGRAM
| .......S.......    \  0  . . . . . . . S . . . . . . .       
| ...............    \  1  . . . . . . . | . . . . . . .
| .......^.......    \  2  . . . . . . | ^ | . . . . . .
| ...............    \  3  . . . . . . | . | . . . . . .
| ......^.^......    \  4  . . . . . | ^ | ^ | . . . . .
| ...............    \  5  . . . . . | . | . | . . . . .
| .....^.^.^.....    \  6  . . . . | ^ | ^ | ^ | . . . .
| ...............    \  7  . . . . | . | . | . | . . . .
| ....^.^...^....    \  8  . . . | ^ | ^ | | | ^ | . . .
| ...............    \  9  . . . | . | . | | | . | . . .
| ...^.^...^.^...    \ 10  . . | ^ | ^ | | | ^ | ^ | . .
| ...............    \ 11  . . | . | . | | | . | . | . .
| ..^...^.....^..    \ 12  . | ^ | | | ^ | | . | | ^ | .
| ...............    \ 13  . | . | | | . | | . | | . | .
| .^.^.^.^.^...^.    \ 14  | ^ | ^ | ^ | ^ | ^ | | | ^ |
| ...............    \ 15  | . | . | . | . | . | | | . |

'S' CONSTANT ENTRY
'^' CONSTANT SPLITTER
'|' CONSTANT BEAM

VARIABLE SPLITS
: SPLIT ( a )   BEAM OVER 1- C!   BEAM SWAP 1+ C!   1 SPLITS +! ;

: BEAM? ( a -- f )  COLS - C@  DUP ENTRY =  SWAP BEAM =  OR ;
: PROPOGATE ( a -- )  DUP C@ SPLITTER = IF  SPLIT  ELSE  BEAM SWAP C!  THEN ;
: PASS  ( row -- )
    ROW COLS BOUNDS DO  I BEAM? IF  I PROPOGATE  THEN  LOOP ;

: PART1 ( -- n )
    SPLITS OFF  ROWS 1 DO  I PASS  LOOP  SPLITS @ ;

\ === PART 2 ===

: CACHE ( r c -- a )  SWAP COLS * + CELLS  PAD + ;
: INIT-CACHE  PAD ROWS COLS * CELLS ERASE
    ROWS 1- COLS 0 DO  1  OVER I CACHE !  LOOP DROP ; ( set last row to 1 )
: .CACHE  ROWS 0 DO CR COLS 0 DO J I CACHE @ 4 .R LOOP LOOP ;

: PATHS  ( row col -- n )  \ CR .S 2DUP AT EMIT SPACE
    2DUP CACHE @ IF  CACHE @ EXIT  THEN  2DUP AT 
    DUP BEAM = IF  DROP  OVER 1+ OVER RECURSE  ELSE
    SPLITTER = IF  OVER 1+ OVER  2DUP 1- RECURSE  -ROT 1+ RECURSE +  ELSE
    0  1 ABORT" paths?"  THEN THEN 
    DUP 2SWAP CACHE ! ;

: ROOT ( -- row col )  1 0  BEGIN 2DUP AT BEAM <> WHILE 1+ REPEAT ;

: PART2 ( -- n )  INIT-CACHE  ROOT PATHS ;

S" example.fs" INCLUDED
T{ PART1 -> 21 }T
T{ PART2 -> 40 }T

S" input.fs" INCLUDED
T{ PART1 -> 1640 }T
T{ PART2 -> 40999072541589 }T
