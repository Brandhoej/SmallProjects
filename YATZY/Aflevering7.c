/* 
 * Andreas Kjeldgaard Brandhoej
 * akbr18@student.aau.dk
 * sw1a412
 * Software 
 */

#include <stdio.h>
#include <stdlib.h> 
#include <time.h>

#define DIE_MAX_EYES (6)
#define BONUSREQUIREDSCORE (63)
#define BONUSSCORE (50)
#define SMALLSTRAIGHTSCORE (15)
#define LARGESTRAIGHTSCORE (20)
#define YATZYSCORE (50)

#define ISUPPERROUND(round) (round < OnePair) ? 1 : 0

typedef int (*int_array_predicate)(int amount);
typedef enum yatzy_round{
  /* Upper section */
  Ones,
  Twos,
  Threes,
  Fours,
  Fives,
  Sixes,
  Total,
  Bonus,
  TotalUpper,
  
  /* Lower section */
  OnePair,
  TwoPairs,
  ThreeOfAKind,
  FourOfAKind,
  SmallStraight,
  LargeStraight,
  FullHouse,
  Chance,
  Yatzy,
  TotalLower,
  GrandTotal
} yatzy_round;

void run_yatzy(void);
int play_yatzy_round(yatzy_round yatzee_round, const int *sorted_die_eyes_counters, int *curr_score);
void scan_roll_count(int *die_count);

void print_yatzy_round(yatzy_round yatzee_round, int scored, int upper_score, int lower_score);
void print_yatzy_roll(const int *sorted_die_eyes_counters);

/* Returns 1 if a roll is required for the round else 0. This is used ion context of Total-rounds */
int yatzy_round_requires_roll(yatzy_round yatzee_round);

/* Rolls the an amount of dies and stores them in sorted_die_eyes_counters where index=0 is the amount of rolled 1's */
void roll_yatzy_round(int rolls, int *sorted_die_eyes_counters);
/* Simulates multiple dies with roll_die */
void roll_multiple_dies(int rolls, int *sorted_die_eyes_counters);
/* Simulates a roll of a die where the highest value is DIE_MAX_EYES */
int roll_die(void);

/* Converts the round enum to a string */
char* yatzy_round_str(yatzy_round round, int normalizespace);

/* Iterates through sorted_die_eyes_counters and looks for n amount of rolls n times at different eye counts.
   The return value is the sum of all n of n's * the eye value at n. Fx. 2's and 5's have n amount. Then return 2n+5n */
int sorted_die_eyes_counters_has_n_of_n(const int *sorted_die_eyes_counters, int amount_of_dies, int amount_to_find);
/* Returns 0 if no full house else the highest score full house is returned. Fx. 6*3+5*2 */
int sorted_die_eyes_counters_has_full_house(const int *sorted_die_eyes_counters);
/* Returns the sum of array[i]*(i+1) where i is the eye number  */
int sum_sorted_die_eyes_counters(const int *array);
/* Iterates through sorted_die_eyes_counters from->to index and returns 1 if all values in that range as atleast a value of 1 */
int sorted_die_eyes_counters_has_consectutive_faces(const int *sorted_die_eyes_counters, int from, int to);

/* Uses the predicate on all elements of sorted_die_eyes_counters and returns the amount which predicated to true  */
int sorted_die_eyes_counters_predicate_continuous(const int *sorted_die_eyes_counters, int target, const int_array_predicate predicate);
/* Uses the predicate from the highest eye to 1 and returns the eye count of the first predicate which is true */
int sorted_die_eyes_counters_predicate(const int *sorted_die_eyes_counters, const int_array_predicate predicate);

/* Predicates  */
int is_pair_p(int amount);
int is_three_of_a_kind_p(int amount);
int is_four_of_a_kind_p(int amount);
int is_yatzee_p(int amount);

int main(void){
  run_yatzy();
  
  return (0);
}

void run_yatzy(void){
  int
    *sorted_die_eyes_counters = calloc(DIE_MAX_EYES, sizeof(int)),
    rolls = 0, round = Ones, upper_score = 0, lower_score, scored = 0;
  
  srand(time(NULL));
  scan_roll_count(&rolls);

  for(round = Ones; round <= GrandTotal; ++round){
    if(yatzy_round_requires_roll(round) != 0){
      roll_yatzy_round(rolls, sorted_die_eyes_counters);
    }
    scored = play_yatzy_round(round, sorted_die_eyes_counters, (ISUPPERROUND(round) != 0) ? &upper_score : &lower_score);
    
    print_yatzy_round(round, scored, upper_score, lower_score);

    if(yatzy_round_requires_roll(round) != 0){
      print_yatzy_roll(sorted_die_eyes_counters);
    }
    
    printf("\n");
  }

  free(sorted_die_eyes_counters);
}

void print_yatzy_round(yatzy_round yatzee_round, int scored, int upper_score, int lower_score){
  char *str_round = yatzy_round_str(yatzee_round, 1);
  switch(yatzee_round){
  case(Total): case(TotalUpper): printf("%s   Total :%-6i", str_round, upper_score); break;
  case(TotalLower):              printf("%s   Total :%-6i", str_round, lower_score); break;
  case(GrandTotal):              printf("%s   Total :%-6i", str_round, upper_score + lower_score); break;
  default: printf("%s   scored:%-6i", str_round, scored); break;
  }
}

void print_yatzy_roll(const int *sorted_die_eyes_counters){
  int eye = 1;
  printf("Roll: ");
  for(eye = 1; eye <= DIE_MAX_EYES; ++eye){
    printf("%i's:%-6i  ", eye, sorted_die_eyes_counters[eye - 1]);
  }
}

int yatzy_round_requires_roll(yatzy_round yatzee_round){
  int req = 1;
  
  switch(yatzee_round){
  case(Total): case(TotalUpper): case(Bonus): case(TotalLower): case(GrandTotal):
    req = 0; break;
  default: break;
  }
  
  return req;
}

int play_yatzy_round(yatzy_round yatzy_round, const int *sorted_die_eyes_counters, int *curr_score){
  int change = 0;
  switch(yatzy_round){
  case(Ones)  : change = sorted_die_eyes_counters[0] * 1; break;
  case(Twos)  : change = sorted_die_eyes_counters[1] * 2; break;
  case(Threes): change = sorted_die_eyes_counters[2] * 3; break;
  case(Fours) : change = sorted_die_eyes_counters[3] * 4; break;
  case(Fives) : change = sorted_die_eyes_counters[4] * 5; break;
  case(Sixes) : change = sorted_die_eyes_counters[5] * 6; break;
    
  case(Bonus) : change = (*curr_score >= BONUSREQUIREDSCORE) ? BONUSSCORE : 0; break;

  case(OnePair)      : change = sorted_die_eyes_counters_predicate(sorted_die_eyes_counters, is_pair_p) * 2; break;
  case(TwoPairs)     : change = sorted_die_eyes_counters_has_n_of_n(sorted_die_eyes_counters, 2, 2); break;
  case(ThreeOfAKind) : change = sorted_die_eyes_counters_predicate(sorted_die_eyes_counters, is_three_of_a_kind_p) * 3; break;
  case(FourOfAKind)  : change = sorted_die_eyes_counters_predicate(sorted_die_eyes_counters, is_four_of_a_kind_p)  * 4; break;
  case(SmallStraight): change = (sorted_die_eyes_counters_has_consectutive_faces(sorted_die_eyes_counters, 1, 5) == 5) ? SMALLSTRAIGHTSCORE : 0; break;
  case(LargeStraight): change = (sorted_die_eyes_counters_has_consectutive_faces(sorted_die_eyes_counters, 2, 6) == 6) ? LARGESTRAIGHTSCORE : 0; break;
  case(FullHouse)    : change = sorted_die_eyes_counters_has_full_house(sorted_die_eyes_counters); break;
  case(Chance)       : change = sum_sorted_die_eyes_counters(sorted_die_eyes_counters); break;
  case(Yatzy)        : change = (sorted_die_eyes_counters_predicate(sorted_die_eyes_counters, is_yatzee_p) != 0) ? YATZYSCORE : 0;  break;

  default: change = 0; break; 
  }
  (*curr_score) += change;
  return change;
}

void scan_roll_count(int* die_count){
  printf("How many dies do you want to simulate?> ");
  scanf(" %i", die_count);
}

void roll_yatzy_round(int rolls, int *sorted_die_eyes_counters){
  int i = 0;
  for(i = 0; i < DIE_MAX_EYES; ++i){
    sorted_die_eyes_counters[i] = 0;
  }
  
  roll_multiple_dies(rolls, sorted_die_eyes_counters);
}

void roll_multiple_dies(int rolls, int *sorted_die_eyes_counters){
  int i = 0;
  for(i = 0; i < rolls; ++i){
    sorted_die_eyes_counters[roll_die() - 1]++;
  }
}

int roll_die(void){
  return (rand() % DIE_MAX_EYES) + 1;
}

char* yatzy_round_str(yatzy_round round, int normalizespace){
  char* str = "";
  
  switch(round){
  case(Ones):          str = (normalizespace != 0) ? "Ones         " : "Ones";          break;
  case(Twos):          str = (normalizespace != 0) ? "Twos         " : "Twos";          break;
  case(Threes):        str = (normalizespace != 0) ? "Threes       " : "Thress";        break;
  case(Fours):         str = (normalizespace != 0) ? "Fours        " : "Fours";         break;
  case(Fives):         str = (normalizespace != 0) ? "Fives        " : "Fives";         break;
  case(Sixes):         str = (normalizespace != 0) ? "Sixes        " : "Sixes";         break;
  case(Total):         str = (normalizespace != 0) ? "Total        " : "Total";         break;
  case(Bonus):         str = (normalizespace != 0) ? "Bonus        " : "Bonus";         break;
  case(TotalUpper):    str = (normalizespace != 0) ? "Total upper  " : "Total upper";   break;
    
  case(OnePair):       str = (normalizespace != 0) ? "OnePair      " : "OnePair";       break;
  case(TwoPairs):      str = (normalizespace != 0) ? "TwoPairs     " : "TwoPairs";      break;
  case(ThreeOfAKind):  str = (normalizespace != 0) ? "ThreeOfAKind " : "ThreeOfAKind";  break;
  case(FourOfAKind):   str = (normalizespace != 0) ? "FourOfAKind  " : "FourOfAKind";   break;
  case(SmallStraight): str = (normalizespace != 0) ? "SmallStraight" : "SmallStraight"; break;
  case(LargeStraight): str = (normalizespace != 0) ? "LargeStraight" : "LargeStraight"; break;
  case(FullHouse):     str = (normalizespace != 0) ? "FullHouse    " : "FullHouse";     break;
  case(Chance):        str = (normalizespace != 0) ? "Chance       " : "Chance";        break;
  case(Yatzy):         str = (normalizespace != 0) ? "Yatzy        " : "Yatzy";         break;
  case(TotalLower):    str = (normalizespace != 0) ? "Total lower  " : "Yatzy";         break;
  case(GrandTotal):    str = (normalizespace != 0) ? "Grand total  " : "Grand total";   break;
  default: break;
  }
  
  return str;
}

int sorted_die_eyes_counters_has_n_of_n(const int *sorted_die_eyes_counters, int amount_of_dies, int amount_to_find){
  int eye = DIE_MAX_EYES, n_found = 0, score = 0;
  while(eye >= 1 && n_found < amount_to_find){
    if(sorted_die_eyes_counters[eye - 1] >= amount_of_dies){
      ++n_found;
      score += eye * amount_to_find;
    }
    --eye;
  }
  return score;
}

int sorted_die_eyes_counters_has_full_house(const int *sorted_die_eyes_counters){
  int eye = DIE_MAX_EYES, has_pair = 0, has_three_of_a_kind = 0;
  while(eye >= 1 && (has_pair == 0 || has_three_of_a_kind == 0)){
    if(is_three_of_a_kind_p(sorted_die_eyes_counters[eye - 1]) != 0 && has_three_of_a_kind == 0){
      has_three_of_a_kind = eye;
    }
    else if(is_pair_p(sorted_die_eyes_counters[eye - 1]) != 0 && has_pair == 0){
      has_pair = eye;
    }
     --eye;
  }
  
  return (has_pair != 0 && has_three_of_a_kind != 0) ? has_pair * 2 + has_three_of_a_kind * 3 : 0;
}

int sum_sorted_die_eyes_counters(const int *array){
  int eye = 0, sum = 0;
  for(eye = 1; eye <= DIE_MAX_EYES; ++eye){
    sum += array[eye - 1] * eye;
  }
  return sum;
}

int sorted_die_eyes_counters_has_consectutive_faces(const int *sorted_die_eyes_counters, int from, int to){
  int eye = from;
  while(sorted_die_eyes_counters[eye - 1] > 0 && eye <= to){
    eye += (from < to) ? 1 : -1; /* Which way do we iterate? */
  }
  return eye - 1;
}

int sorted_die_eyes_counters_predicate_continuous(const int *sorted_die_eyes_counters, int target, const int_array_predicate predicate){
  int counter = 0, eye = DIE_MAX_EYES;
  while(eye >= 1 && predicate(sorted_die_eyes_counters[eye - 1]) == 0 && counter < target){
    --eye; ++counter;
  }
  return counter;
}

int sorted_die_eyes_counters_predicate(const int *sorted_die_eyes_counters, const int_array_predicate predicate){
  int eye = DIE_MAX_EYES;
  while(eye >= 1 && predicate(sorted_die_eyes_counters[eye - 1]) == 0){
    --eye;
  }
  return eye;
}

int is_pair_p(int amount){
  return amount >= 2;
}

int is_three_of_a_kind_p(int amount){
  return amount >= 3;
}

int is_four_of_a_kind_p(int amount){
  return amount >= 4;
}

int is_yatzee_p(int amount){
  return amount >= 5;
}
