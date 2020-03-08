/* 
 * Andreas Kjeldgaard Brandhoej
 * akbr18@student.aau.dk
 * sw1a412
 * Software 
 */

/*
 * :::::How to run with arguments:::::
 * argv[1]=="-L%i", where %i is the size of the linked hashtable (should never be negative)
 * argv[2]=="-D", DEBUGMODE: prints the linked hash table with the teams before printing the sorted order
 */

/*
 * :::::NOTES:::::
 * Calloc is used to ensure that the allocated block is zerofilled.
 * This makes it reliable to check for null pointers.
 */

/*
 * :::::Process:::::
 * This application reads a file with league soccer matches from 2018-2019
 * 1. A linked list is constructed with all matches
 * 2. The matches are then looped and teams are searched for and constructed 
 *      The teams are placed in a linked hashtable
 * 3. Matches are then looped and teams get their points for every match
 * 4. If argv[2]=="-D"
 *      Yes: The raw linked hashtable is printed
 * 5. Teams are then sorted in a seperate array
 * 6. Teams get printed in order
 * 7. Everything is freed
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FILEPATH "kampe-2018-2019.txt"
#define STR_MAX_LEN (100)
#define MATCH_WIN_POINTS (3)
#define MATCH_WIN_DRAW (1)
#define MATCH_WIN_LOSE (0)

/* Comparator */
typedef int (cmp)(const void *a, const void *b);
/* Generic toString */
typedef char* (toString)(const void *v);
/* Functions used in the context of freeing blocks */
typedef void (destroy)(void *v);

typedef unsigned int (getHash)(const void *v); 
/* Winning points are returned if s1 > s2 */
typedef unsigned int (FBMatchTeamPoint)(unsigned int s1, unsigned int s2);

typedef struct LinkedNode{
    void *value;
    struct LinkedNode *next;
} LinkedNode;

typedef struct LinkedHashTable{
    unsigned int tableSize;
    LinkedNode *table;
} LinkedHashTable;

typedef struct KeyValuePair{
    void *key, *value;
} KeyValuePair;

typedef struct FBTeam{
    char *name;
    unsigned int points, goalsFor, goalsAgainst;
} FBTeam;

typedef struct FBMatch{
    char weekday[5], homeTeam[5], outTeam[5];
    unsigned int hour, min, day, month, homeScore, outScore;
    unsigned int spectators;
} FBMatch;

/*----------IO----------*/
/* Loads teams and matches into two seperate linked lists. The file path used is FILEPATH. 
 * The expected values at the origin nodes is NULL. Returns the amount of matches loaded. */
int LoadFBMatches(LinkedNode *matches);


/*----------FBMatch----------*/
/* Converts a string to a FBTeam and returns 1 if succes */
int ConvertStrToFBMatch(const char *str, FBMatch *match);
/* Returns the amount of points for s1 */
unsigned int FBMatchesScore(unsigned int s1, unsigned int s2);
char *FBMatchToString(const void *match);
unsigned int GetFBMatchHash(const void *match);
void freeFBMatch(void *match);


/*----------FBTeam----------*/
/* Creates a linked hashtable with teams and returns the amount of teams. */
int GetTeamsFromMatches(const LinkedNode *matches, LinkedHashTable *teams, getHash getHash, cmp comparator);
/* Loops through ever match and adds points and goals for the team */
void GetTeamsScoresAndPointsFromMatches(const LinkedNode *matches, LinkedHashTable *teams, FBMatchTeamPoint getMatchPoint, getHash getHash, cmp comparator);
char *LinkedFBTeamToString(const void *linkedTeam);
char *FBTeamToString(const void *team);
/* Returns the hashcode for a FBTeam struct */
unsigned int GetFBTeamHash(const void *teamName);
/* Compares two teams by each others.
 * The void pointers are typecasted to FBTeam* */
int CmpLinkedFBTeams(const void *t1, const void *t2);
/* Compares a team name with a team. 
 * This is usefull if the user wants to find a team in the hashtable
 * but the values in the hashtable are of type FBTeam 
 * The name is typecasted to char* and team to FBTeam* */
int CmpTeamNameByTeam(const void *name, const void *team);
/* Compares two teams by name. The void pointers are type casted to FBTeam */
int CmpTeamsByName(const void *a, const void *b);
void FreeFBTeam(void *team);


/*----------LinkedHashTable----------*/
/* Initializes a linked hashtable with a size */
void InitLinkedHashTable(LinkedHashTable *hashTable, unsigned int tableSize);
/* Returns the index of the table the value was added to */
int AddToLinkedHashTable(const LinkedHashTable *hashTable, void *value, getHash getHash);
/* Returns the amount of elements or linked nodes in the table */
int GetElementCountInLinkedHashTable(const LinkedHashTable *table);
/* Returns 1 if the value was found in the table */
int HashTableContains(const LinkedHashTable *hashtable, void *value, cmp compare, getHash hash);
/* Returns the found linkedNode else NULL */
LinkedNode *FindInHashTable(const LinkedHashTable *hashtable, void *value, cmp compare, getHash hash);
/* A way of subscripting the graph. Returns NULL if the index was out of bounds */
LinkedNode *LinkedHashTableSubScript(const LinkedHashTable *hashtable, int n);
/* Convert the hashtable to an array. The order of elements is the same as with LinkedHashTableSubScript */
LinkedNode *LinkedHashTableToArray(const LinkedHashTable *table, int *length);
void PrintLinkedHashTable(const LinkedHashTable *table, toString toString);
void FreeLinkedHashTable(LinkedHashTable *table, destroy destroy);


/*----------LinkedList----------*/
/* Creates a new node and sets the value of the node to the value 
 * and the prev->next to the new node and then returns the node */
LinkedNode *AddToLinkedList(LinkedNode *prev, void *value);
/* Returns the found linkedNode else NULL */
LinkedNode *FindLinkedNode(LinkedNode *origin, void *value, cmp comparator);
/* Returns the last LinkedNode in the list starting from node */
LinkedNode *GetEndOfLinkedList(LinkedNode *node);
/* Returns the nth node else NULL */
LinkedNode *GetNthNode(LinkedNode *origin, int n);
/* Returns the amount of elements linked together */
int GetLinkedListLength(LinkedNode *origin);
/* Returns 1 if team is in the linked list */
int LinkedListContains(const LinkedNode *node, void *item, cmp cmp);
/* Prints a linked list */
void PrintLinkedList(const LinkedNode *origin, toString toString);
void PrintArrayOfLinkedNodes(const LinkedNode array[], int length, toString toString);
void FreeLinkedList(LinkedNode *origin, destroy destroy);


/*----------Generele Hashing----------*/
/* Interface for GetStrHash. calls GetStrHash where void* is casted to char* */
unsigned int GetVoidStrHash(const void *str);
/* Returns the hashcode for a string */
unsigned int GetStrHash(const char *str);


/*----------Generic Comparators----------*/
/* Returns 1 if strcmp of the strings returns 0 else this returns 0 */
int CmpStr(const void *a, const void *b);


int main(int argc, char *argv[]){
    LinkedNode *matches = calloc(1, sizeof(LinkedNode));
    LinkedHashTable *teams = calloc(1, sizeof(LinkedHashTable));
    LinkedNode *teamArray = NULL;
    int linkedHashTableLength = 10, teamArrayLength = 0;
    
    if(argc >= 2){
        printf("%s\n", argv[1]);
        sscanf(argv[1], "-L%i", &linkedHashTableLength);
    }
    printf("Length::%i\n", linkedHashTableLength);
    
    InitLinkedHashTable(teams, linkedHashTableLength);
    LoadFBMatches(matches);
    
    GetTeamsFromMatches(matches, teams, GetFBTeamHash, CmpTeamsByName);
    GetTeamsScoresAndPointsFromMatches(matches, teams, FBMatchesScore, GetVoidStrHash, CmpTeamNameByTeam);
    
    if(argc >= 3){
        if(strcmp(argv[2], "-D") == 0){
            PrintLinkedHashTable(teams, FBTeamToString);
            printf("\n\n");
        }
    }
    
    teamArray = LinkedHashTableToArray(teams, &teamArrayLength);
    qsort(teamArray, teamArrayLength, sizeof(LinkedNode), CmpLinkedFBTeams);

    printf("Sorted by team scores:\n");
    PrintArrayOfLinkedNodes(teamArray, teamArrayLength, LinkedFBTeamToString);
    
    /* The inner members of the teams will be removed with FreeLinkedHashTable */
    free(teamArray);
    FreeLinkedHashTable(teams, FreeFBTeam);
    FreeLinkedList(matches, freeFBMatch);
    
    return EXIT_SUCCESS;
}

int LoadFBMatches(LinkedNode *matches){
    char str[STR_MAX_LEN];
    FILE *f = fopen(FILEPATH, "r");
    LinkedNode *currMatch = (matches == NULL) ? calloc(1, sizeof(LinkedNode)) : matches;
    int amount = 0;
    
    while(fgets(str, STR_MAX_LEN, f) != NULL){
        FBMatch *match = calloc(1, sizeof(FBMatch));
        ConvertStrToFBMatch(str, match);
        
        if(amount == 0){
            matches->value = match;
        }
        else{
            currMatch = AddToLinkedList(currMatch, match);
        }
        
        amount++;
    }
    
    fclose(f);
    return amount;
}

int ConvertStrToFBMatch(const char *str, FBMatch *match){
    int t = 0;
    t = sscanf(str, " %s %u/%u %u.%u %s - %s %u - %u %u",
        match->weekday, &(match->day), &(match->month), 
        &(match->hour), &(match->min),
        match->homeTeam, match->outTeam, 
        &(match->homeScore), &(match->outScore),
        &(match->spectators));
    return t;
}

unsigned int FBMatchesScore(unsigned int s1, unsigned int s2){
    unsigned int score = MATCH_WIN_LOSE;
    if(s1 > s2){
        score = MATCH_WIN_POINTS;
    }
    else if(s1 == s2){
        score = MATCH_WIN_DRAW;
    }
    return score;
}

char *FBMatchToString(const void *match){
    FBMatch *fbm = (FBMatch*)match;
    char *str = calloc(STR_MAX_LEN, sizeof(char));
    sprintf(str, "%3s   %02u/%02u   %02u.%02u   %3s - %-3s   %2u - %-2u  Specs: %u",
        fbm->weekday, fbm->day, fbm->month,
        fbm->hour, fbm->min,
        fbm->homeTeam, fbm->outTeam,
        fbm->homeScore, fbm->outScore,
        fbm->spectators);
    return str;
}

unsigned int GetFBMatchHash(const void *match){
    char str[STR_MAX_LEN];
    /* day+min concatenated with month+hour
     * It is the sum because i think min and hour should have influence 
     * And it is not possible to make "min hour day month" 
     * Because if min is 00 then we dont use decimal */
    sprintf(str, "%i%i", ((FBMatch*)match)->day + ((FBMatch*)match)->min, ((FBMatch*)match)->month + ((FBMatch*)match)->hour);
    return GetStrHash(str);
}

void freeFBMatch(void *match){
    FBMatch *fbm = (FBMatch*)match;
    free(&fbm->weekday);
    free(&fbm->homeTeam);
    free(&fbm->outTeam);
    free(fbm);
}

int GetTeamsFromMatches(const LinkedNode *matches, LinkedHashTable *teams, getHash getHash, cmp compare){
    static int res = 0;
    FBTeam *homeTeam = calloc(1, sizeof(FBTeam)), *outTeam = calloc(1, sizeof(FBTeam));
    if(matches != NULL){
        FBMatch *match = (FBMatch*)matches->value;
        homeTeam->name = match->homeTeam;
        outTeam->name = match->outTeam;
        
        if(!HashTableContains(teams, homeTeam, compare, getHash)){
            AddToLinkedHashTable(teams, homeTeam, getHash);
            homeTeam = calloc(1, sizeof(FBTeam));
            res++;
        }
        
        if(!HashTableContains(teams, outTeam, compare, getHash)){
            AddToLinkedHashTable(teams, outTeam, getHash);
            outTeam = calloc(1, sizeof(FBTeam));
            res++;
        }
        
        GetTeamsFromMatches(matches->next, teams, getHash, compare);
    }
    return res;
}

void GetTeamsScoresAndPointsFromMatches(const LinkedNode *matches, LinkedHashTable *teams, FBMatchTeamPoint getMatchPoint, getHash getHash, cmp comparator){
    FBMatch *match = (FBMatch*)matches->value;
    FBTeam
        *homeTeam = (FBTeam*)FindInHashTable(teams, match->homeTeam, comparator, getHash)->value,
        *outTeam  = (FBTeam*)FindInHashTable(teams, match->outTeam,  comparator, getHash)->value;
    
    homeTeam->goalsFor     += match->homeScore;
    homeTeam->goalsAgainst += match->outScore;
    homeTeam->points       += getMatchPoint(match->homeScore, match->outScore);
    
    outTeam->goalsFor     += match->outScore;
    outTeam->goalsAgainst += match->homeScore;
    outTeam->points       += getMatchPoint(match->outScore, match->homeScore);
    
    if(matches->next != NULL){
        GetTeamsScoresAndPointsFromMatches(matches->next,teams, getMatchPoint, getHash, comparator);
    }
}

char *LinkedFBTeamToString(const void *linkedTeam){
    return FBTeamToString(((LinkedNode*)linkedTeam)->value);
}

char *FBTeamToString(const void *team){
    FBTeam *fbt = (FBTeam*)team;
    char *str = calloc(STR_MAX_LEN, sizeof(char));
    sprintf(str, "%3s   goals: %2u-%-2u  points: %2u",
        fbt->name, fbt->goalsFor, fbt->goalsAgainst, fbt->points);
    return str;
}

unsigned int GetFBTeamHash(const void *team){
    return GetStrHash(((FBTeam*)team)->name);
}

int CmpLinkedFBTeams(const void *t1, const void *t2){
    int cmp = 0;
    FBTeam 
        *ft1 = (FBTeam*)((LinkedNode*)t1)->value,
        *ft2 = (FBTeam*)((LinkedNode*)t2)->value;
    
    if(ft1->points != ft2->points){
        cmp = (ft1->points > ft2->points) ? -1 : 1;
    }
    else if(ft1->goalsFor != ft2->goalsFor){
        cmp = (ft1->goalsFor > ft2->goalsFor) ? -1 : 1;
    }
    else if(ft1->goalsAgainst != ft2->goalsAgainst){
        cmp = (ft1->goalsAgainst < ft2->goalsAgainst) ? -1 : 1;
    }
    
    return cmp;
}

int CmpTeamNameByTeam(const void *name, const void *team){
    return CmpStr((char*)name, ((FBTeam*)team)->name);
}

int CmpTeamsByName(const void *a, const void *b){
    return CmpStr(((FBTeam*)a)->name, ((FBTeam*)b)->name);
}

void FreeFBTeam(void *team){
    FBTeam *fbt = (FBTeam*)team;
    free(fbt->name);
    free(fbt);
}

void InitLinkedHashTable(LinkedHashTable* hashTable, unsigned int tableSize){
    if(tableSize != 0){
        hashTable->tableSize = tableSize;
        hashTable->table = calloc(tableSize, sizeof(LinkedNode));
    }
}

int AddToLinkedHashTable(const LinkedHashTable *hashTable, void *value, getHash getHash){
    unsigned int tableIndex = getHash(value) % hashTable->tableSize;
    AddToLinkedList(&hashTable->table[tableIndex], value);
    return tableIndex;
}

int GetElementCountInLinkedHashTable(const LinkedHashTable *table){
    int count = 0, i = 0;
    for(i = 0; i < table->tableSize; ++i){
        count += GetLinkedListLength(&table->table[i]);
    }
    return count;
}

int HashTableContains(const LinkedHashTable *hashtable, void *value, cmp compare, getHash getHash){
    unsigned int tableIndex = getHash(value) % hashtable->tableSize;
    return LinkedListContains(&hashtable->table[tableIndex], value, compare);
}

LinkedNode *FindInHashTable(const LinkedHashTable *hashtable, void *value, cmp compare, getHash hash){
    unsigned int tableIndex = hash(value) % hashtable->tableSize;
    return FindLinkedNode(&hashtable->table[tableIndex], value, compare);
}

LinkedNode *LinkedHashTableSubScript(const LinkedHashTable *hashtable, int n){
    LinkedNode *node = NULL;
    int tableIndex = 0, currAmount = 0;
    
    while(n >= currAmount){
        currAmount = GetLinkedListLength(&hashtable->table[tableIndex]);
        
        if(n >= currAmount){
            n -= currAmount;
            ++tableIndex;
            currAmount = 0;
        }
    }
    
    if(tableIndex < hashtable->tableSize){
        node = GetNthNode(&hashtable->table[tableIndex], n);
    }
    
    return node;
}

LinkedNode *LinkedHashTableToArray(const LinkedHashTable *table, int *size){
    LinkedNode *arr = NULL;
    int i = 0; 
    *size = GetElementCountInLinkedHashTable(table);
    
    arr = calloc(*size, sizeof(LinkedNode));
    for(i = 0; i < *size; ++i){
        arr[i] = *LinkedHashTableSubScript(table, i);
        arr[i].next = NULL;
    }
    
    return arr;
}

void PrintLinkedHashTable(const LinkedHashTable *table, toString toString){
    int i = 0;
    for(i = 0; i < table->tableSize; ++i){
        printf("-----%03i-----\n", i);
        PrintLinkedList(&table->table[i], toString);
    }
    printf("-----END-----\n");
}

void FreeLinkedHashTable(LinkedHashTable *table, destroy destroy){
    LinkedNode *node = NULL;
    int i = 0;
    for(i = 0; i < table->tableSize; ++i){
        node = &table->table[i];
        if(node != NULL && node->value != NULL)
        {
            FreeLinkedList(node, destroy);
        }
    }
    free(table->table);
    free(table);
}

LinkedNode *AddToLinkedList(LinkedNode *prev, void *value){
    LinkedNode *after = prev->next;
    /* If the node we actually try to append on has a NULL value then set that instead of extending */
    if(prev->value == NULL){
        prev->value = value;
    }
    /* IF NULL then we are appending on the end of th elinked list */
    else if(after == NULL){
        after = calloc(1, sizeof(LinkedNode));
        after->value = value;
        prev->next = after;        
    }
    /* If not NULL then we are appending in between two nodes */
    else if(after != NULL){
        LinkedNode *curr = calloc(1, sizeof(LinkedNode));
        curr->value = value;
        curr->next = after;
        prev->next = curr;
    }
    return after;
}

LinkedNode *FindLinkedNode(LinkedNode *origin, void *value, cmp comparator){
    LinkedNode *node = NULL;
    if(origin != NULL){
        if(origin->value != NULL){
            if(comparator(value, origin->value)){
                node = origin;
            }
        }
        if(node == NULL && origin->next != NULL){
            node = FindLinkedNode(origin->next, value, comparator);
        }
    }
    return node;
}

LinkedNode *GetEndOfLinkedList(LinkedNode *node){
    LinkedNode *res = node;
    if(node->next != NULL){
        res = GetEndOfLinkedList(node->next);
    }
    return res;
}

LinkedNode *GetNthNode(LinkedNode *origin, int n){
    LinkedNode *node = origin;
    while (n > 0 && node->next != NULL)
    {
        node = node->next;
        --n;
    }
    return n == 0 ? node : NULL;
}

int GetLinkedListLength(LinkedNode *origin){
    int length = 0;
    /* If the origin node it not set then this linked node is an 
     * empty origin and the next node should not be considered */
    if(origin->value != NULL){
        LinkedNode *curr = origin;
        while(curr != NULL){
            curr = curr->next;
            ++length;
        }
    }
    return length;
}

int LinkedListContains(const LinkedNode *node, void *item, cmp cmp){
    int found = 0;
    if(node != NULL){
        /* A node can have a child and a NULL value. */
        if(node->value != NULL){
            found = cmp(item, node->value);
        }
        if(found == 0 && node->next != NULL){
            found = LinkedListContains(node->next, item, cmp);
        }
    }
    return found;
}

void PrintLinkedList(const LinkedNode *origin, toString toString){
    if(origin->value != NULL){
        printf("%s\n", toString(origin->value));
    }
    if(origin->next != NULL){
        PrintLinkedList(origin->next, toString);
    }
}

void PrintArrayOfLinkedNodes(const LinkedNode array[], int length, toString toString){
    int i = 0;
    for(i = 0; i < length; ++i){
        printf("%s\n", toString(&array[i]));
    }
}

void FreeLinkedList(LinkedNode *origin, destroy destroy){
    if(origin != NULL){
        FreeLinkedList(origin->next, destroy);
        if(origin->value != NULL){
            destroy(origin->value);
        }
        free(origin);
    }
}

unsigned int GetVoidStrHash(const void *str){
    return GetStrHash((char*)str);
}

unsigned int GetStrHash(const char *str){
    unsigned int hash = 0;
    char c;
    /* Sum all characters. The modulo depends on the collection
     * Therefore this operation does not happen here. */
    while((c = *str) != '\0'){
        hash += c; str++;
    }
    return hash;
}

int CmpStr(const void *a, const void *b){
    return strcmp((char*)a, (char*)b) == 0 ? 1 : 0;
}