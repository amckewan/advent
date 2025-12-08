\ Day 7, Part 1

include ../init.fs

0 VALUE START
0 VALUE ROWS
0 VALUE COLS

: DIAGRAM   ALIGN  HERE TO START  0 TO ROWS  0 TO COLS ;
: |         BL PARSE  DUP TO COLS  HERE SWAP DUP ALLOT CMOVE
            ROWS 1+ TO ROWS ;

: ROW ( n -- a )  COLS * START + ;
: D   ROWS 0 DO  CR I ROW COLS TYPE  LOOP ;

DIAGRAM
| .......S.......    \ . . . . . . . S . . . . . . .       
| ...............    \ . . . . . . . | . . . . . . .
| .......^.......    \ . . . . . . | ^ | . . . . . .
| ...............    \ . . . . . . | . | . . . . . .
| ......^.^......    \ . . . . . | ^ | ^ | . . . . .
| ...............    \ . . . . . | . | . | . . . . .
| .....^.^.^.....    \ . . . . | ^ | ^ | ^ | . . . .
| ...............    \ . . . . | . | . | . | . . . .
| ....^.^...^....    \ . . . | ^ | ^ | | | ^ | . . .
| ...............    \ . . . | . | . | | | . | . . .
| ...^.^...^.^...    \ . . | ^ | ^ | | | ^ | ^ | . .
| ...............    \ . . | . | . | | | . | . | . .
| ..^...^.....^..    \ . | ^ | | | ^ | | . | | ^ | .
| ...............    \ . | . | | | . | | . | | . | .
| .^.^.^.^.^...^.    \ | ^ | ^ | ^ | ^ | ^ | | | ^ |
| ...............    \ | . | . | . | . | . | | | . |

'S' CONSTANT ENTRY
'^' CONSTANT SPLITTER
'|' CONSTANT BEAM

VARIABLE SPLITS
: SPLIT ( a )   BEAM OVER 1- C!   BEAM SWAP 1+ C!   1 SPLITS +! ;

: BEAM? ( a -- f )  COLS - C@  DUP ENTRY =  SWAP BEAM =  OR ;
: PROPOGATE ( a -- )  DUP C@ SPLITTER = IF  SPLIT  ELSE  BEAM SWAP C!  THEN ;
: PASS  ( row -- )
    ROW COLS BOUNDS DO  I BEAM? IF  I PROPOGATE  THEN  LOOP ;

: SOLVE ( -- n )
    SPLITS OFF  ROWS 1 DO  I PASS  LOOP  SPLITS @ ;

S" example.fs" INCLUDED
T{ SOLVE -> 21 }T

S" input.fs" INCLUDED
T{ SOLVE -> 1640 }T
