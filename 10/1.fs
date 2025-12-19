\ Day 10, part 1

include ../init.fs

\ ============================================================
\ Parsing

\  [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
\  [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
\  [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
\   012345

: parse-lights ( a n -- x )  \ [.###.#]
    1- + ( remove ] )  0 ( x) swap
    begin   ( x a )  1-  dup c@  dup '[' xor while
        '#' = negate  rot 1 lshift  or ( set bit )  swap
    repeat 2drop ;

t{ s" [.##.]" parse-lights -> %0110 }t
t{ s" [...#.]" parse-lights -> %01000 }t
t{ s" [.###.#]" parse-lights -> %101110 }t

: setbit ( n x -- x )  1 rot lshift or ;

: number ( a n -- a' n' n )
    0 0 2swap >number  rot drop rot ;

: parse-button ( a n -- x )   \ (0,1,23,14,5)
    0 >r ( x )
    begin   over c@ ')' xor  over and while
            1 /string  number  r> setbit >r
    repeat  2drop r> ;

t{ s" (2,3)" parse-button -> %1100 }t
t{ s" (1,3,5)" parse-button -> %101010 }t
t{ s" (0,1,2,3,4)" parse-button -> %11111 }t

\ ============================================================
\ parse machine
\ [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}

0 value lights
0 value #buttons
create  buttons   20 cells allot    ( max 7 buttons for input )

: button ( n -- x )  cells buttons + @ ;

: add-button ( x -- )  #buttons  dup 1+ to #buttons  cells buttons + ! ;

: parse-machine
    0 to #buttons
    begin   parse-name  dup while
        over c@ case
        '[' of  parse-lights  to lights   endof
        '(' of  parse-button  add-button  endof
                2drop
        endcase
    repeat  2drop ;

t{ parse-machine [.##.] (3) (1,3) (2) {3,5,4,7}
   -> }t
t{ lights   -> %0110 }t
t{ 0 button -> %1000 }t
t{ 1 button -> %1010 }t
t{ 2 button -> %0100 }t

\ ============================================================
\ Puzzle

: machine:   parse-machine ;

machine: [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}

: w@ uw@ ; \ gforth

8192 constant #patterns
create bits   #patterns 2*  dup allot  bits swap erase

marker task
: #bits ( x -- n )
    0 begin over while ( x n )
        over 1 and +  swap 2/ swap
    repeat nip ;

t{ 0 #bits -> 0 }t
t{ 1 #bits -> 1 }t
t{ 16 #bits -> 1 }t
t{ %1001101010110 #bits -> 7 }t
t{ 8191 #bits -> 13 }t
t{ 8190 #bits -> 12 }t

:noname  #patterns 0 do  i #bits  i 2* bits + w!  loop ; execute
task

: #bits ( x -- n )  2* bits + w@ ;

t{ 0 #bits -> 0 }t
t{ 1 #bits -> 1 }t
t{ 16 #bits -> 1 }t
t{ %1001101010110 #bits -> 7 }t
t{ 8191 #bits -> 13 }t
t{ 8190 #bits -> 12 }t

\ Try all patterns, find ones with the fewest buttons
: apply ( pattern -- x )   0 ( x ) swap
    #buttons 0 do
        dup 1 and if  ( press ) i button  rot xor swap  then
        2/
    loop drop ;

: fewest ( -- n )  9999 ( fewest # buttons required )
    #patterns 0 do
        i #bits over < if ( fewer buttons, see if it works )
            i apply lights = if  drop i #bits  then
        then
    loop ;

machine: [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
t{ fewest -> 2 }t
machine: [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
t{ fewest -> 3 }t
machine: [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
t{ fewest -> 2 }t

variable answer

: machine:  parse-machine  fewest answer +! ;

answer off
include input.fs

: solve  answer ? cr bye ;
