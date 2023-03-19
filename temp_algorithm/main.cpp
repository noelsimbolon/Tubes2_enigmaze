#include <bits/stdc++.h>
using namespace std;

static int bfsCount = 0;
static int dfsCount = 0;
static vector<char> possibleBFS;
static vector<char> possibleDFS;

pair<int,int> findStart(vector<vector<char>> grid){
    int n = grid.size();
    int m = grid[0].size();
    for(int i = 0; i < n; i++){
        for(int j = 0; j < m; j++){
            if(grid[i][j] == 'K'){
                return {j,i};
            }
        }
    }
    return {-1,-1};
}

int countTreasure(vector<vector<char>> grid){
    int n = grid.size();
    int m = grid[0].size();
    int count = 0;
    for(int i = 0; i < n; i++){
        for(int j = 0; j < m; j++){
            if(grid[i][j] == 'T'){
                count++;
            }
        }
    }
    return count;
}

// TO DO : check if it's fitting to use for the pred used in bfs
void memoizePath(vector<char> &ans, vector<vector<pair<int,int>>> &pred, pair<int,int> start){
    vector<char> temp = ans;
    ans.clear();
    while(pred[start.second][start.first].first != -1){
        if(pred[start.second][start.first].first < start.first){
            ans.push_back('R');
        }
        else if(pred[start.second][start.first].first > start.first){
            ans.push_back('L');
        }
        else if(pred[start.second][start.first].second < start.second){
            ans.push_back('D');
        }
        else if(pred[start.second][start.first].second > start.second){
            ans.push_back('U');
        }
        start = pred[start.second][start.first];
    }
    while(temp.size() > 0){
        ans.push_back(temp.back());
        temp.pop_back();
    }
    reverse(ans.begin(), ans.end());
    pred.assign(pred.size(), vector<pair<int,int>>(pred[0].size(), {-1,-1})); // reset pred since we can revisit nodes
}

void bfs(vector<vector<char>> grid, pair<int,int> start, int &treasure, vector<vector<bool>> &visited, vector<vector<pair<int,int>>> &pred, vector<char> ans, bool tsp = false){
    if(possibleBFS.size() > 0){
        return;
    }

    // get datas
    int n = grid.size(); // row size
    int m = grid[0].size(); // col size
    int x = start.first; // current x coordinate
    int y = start.second; // current y coordinate

    queue<pair<int,int>> q;
    q.push(start);
    visited[y][x] = true;
    pred[y][x] = {-1,-1};

    while(!q.empty()){
        bfsCount++;
        x = q.front().first;
        y = q.front().second;
        q.pop();

        // if treasure found
        if(grid[y][x] == 'T'){
            treasure--;
            grid[y][x] = 'R';
            visited.assign(n, vector<bool>(m, false)); // reset visited since we can revisit nodes
            visited[y][x] = true;
            memoizePath(ans, pred, {x,y});
            break;
        }

        if(y-1 >= 0){
            if(grid[y-1][x]!='X' && !visited[y-1][x]){
                q.push({x,y-1});
                visited[y-1][x] = true;
                pred[y-1][x] = {x,y};
            }
        }

        if(x+1 < m){
            if(grid[y][x+1]!='X' && !visited[y][x+1]){
                q.push({x+1,y});
                visited[y][x+1] = true;
                pred[y][x+1] = {x,y};
            }
        }

        if(y+1 < n){
            if(grid[y+1][x]!='X' && !visited[y+1][x]){
                q.push({x,y+1});
                visited[y+1][x] = true;
                pred[y+1][x] = {x,y};
            }
        }

        if(x-1 >= 0){
            if(grid[y][x-1]!='X' && !visited[y][x-1]){
                q.push({x-1,y});
                visited[y][x-1] = true;
                pred[y][x-1] = {x,y};
            }
        }
    }
    if(treasure == 0){
        if(tsp){
            pair<int,int> krusty = findStart(grid);
            grid[krusty.second][krusty.first] = 'T';
            treasure++;
            cout<< x << " " << y << endl;
            cout << krusty.first << " " << krusty.second << endl;
            bfs(grid, {y,x}, treasure, visited, pred, ans);
        }
        else {
            possibleBFS = ans;
        }
    }
    if(treasure>0){
        bfs(grid, {x,y}, treasure, visited, pred, ans, tsp);
    }
}

void dfs(vector<vector<char>> grid, pair<int,int> start, int treasure, vector<char> ans, vector<vector<bool>> visited, bool tsp = false){
    if(possibleDFS.size() > 0){
        return;
    }

    dfsCount++;

    // get datas
    int n = grid.size(); // row size
    int m = grid[0].size(); // col size
    int x = start.first; // current x coordinate
    int y = start.second; // current y coordinate

    // if treasure found
    if(grid[y][x] == 'T'){
        treasure--;
        grid[y][x] = 'R';
        visited.assign(n, vector<bool>(m, false)); // reset visited since we can revisit nodes
    }

    // if all treasure found
    if(treasure == 0){
        if(tsp){
            pair<int,int> krusty = findStart(grid);
            grid[krusty.second][krusty.first] = 'T';
            dfs(grid, start, 1, ans, visited);
        }
        else {
            possibleDFS = ans;
        }
        return;
    }

    // dfs call stacks
    visited[y][x] = true;
        if(y-1 >= 0 && possibleDFS.size() == 0){
            if(grid[y-1][x]!='X' && !visited[y-1][x] && y-1 >= 0){
                ans.push_back('U');
                dfs(grid, {x,y-1}, treasure, ans, visited, tsp);
                ans.pop_back();
            }
        }

        if(x+1 < m && possibleDFS.size() == 0){
            if(grid[y][x+1]!='X' && !visited[y][x+1]){
                ans.push_back('R');
                dfs(grid, {x+1,y}, treasure, ans, visited, tsp);
                ans.pop_back();
            }
        }
    
        if(y+1 < n && possibleDFS.size() == 0){
            if(grid[y+1][x]!='X' && !visited[y+1][x]){
                ans.push_back('D');
                dfs(grid, {x,y+1}, treasure, ans, visited, tsp);
                ans.pop_back();
            }
        }

        if(x-1 >= 0 && possibleDFS.size() == 0){
            if(grid[y][x-1] != 'X' && !visited[y][x-1]){
                ans.push_back('L');
                dfs(grid, {x-1,y}, treasure, ans, visited, tsp);
                ans.pop_back();
            }
        }
}

int main(){
    vector<vector<char>> grid = {{'K','R','R','R'},{'X','R','X','T'},{'X','T','R','R'},{'X','R','X','X'}};
    pair<int,int> start = findStart(grid);
    int treasure = countTreasure(grid);
    vector<vector<bool>> visited(grid.size(), vector<bool>(grid[0].size(), false));

    // dfs(grid, start, treasure, {}, visited, true);
    // for(auto i : possibleDFS){
    //     cout << i << " ";
    // }
    // cout << endl;
    // cout << "Nodes: " << dfsCount << endl;
    // cout << "Steps: " << possibleDFS.size() << endl;

    cout << "BFS" << endl;
    vector<vector<pair<int,int>>> pred;
    pred.assign(grid.size(), vector<pair<int,int>>(grid[0].size(), {-1,-1}));
    bfs(grid, start, treasure, visited, pred, {}, true);
    for(auto i : possibleBFS){
        cout << i << " ";
    }
    cout << endl;

    // vector<vector<pair<int,int>>> pred =
    // {
    //     {{-1,-1},{0,0},{-1,-1},{-1,-1}},
    //     {{0,0},{0,1},{1,1},{2,1}},
    //     {{-1,-1},{-1,-1},{-1,-1},{3,1}},
    //     {{-1,-1},{-1,-1},{-1,-1},{3,2}}
    // };

    // vector<char> ans = {'U', 'L'};
    // memoizePath(ans, pred, {3,3});

    // for(auto i : ans){
    //     cout << i << " ";
    // }

    return 0;
}