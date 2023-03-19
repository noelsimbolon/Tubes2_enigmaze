#include <bits/stdc++.h>
using namespace std;

static int bfsCount = 0;
static int dfsCount = 0;
static vector<vector<char>> possibleBFS;
static vector<vector<char>> possibleDFS;

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

vector<char> findOptimal(vector<vector<char>> possible){
    int min = INT_MAX;
    vector<char> ans;
    for(int i = 0; i < possible.size(); i++){
        if(possible[i].size() < min){
            min = possible[i].size();
            ans = possible[i];
        }
    }
    return ans;
}

vector<char> convertToPath(vector<pair<int,int>> vec){
        vector<char> ans;
        for(int i = 0; i < vec.size()-1; i++){
            if(vec[i].first == vec[i+1].first){
                if(vec[i].second < vec[i+1].second){
                    ans.push_back('D');
                }
                else{
                    ans.push_back('U');
                }
            }
            else{
                if(vec[i].first < vec[i+1].first){
                    ans.push_back('R');
                }
                else{
                    ans.push_back('L');
                }
            }
        }
        return ans;
}

vector<pair<int,int>> getPath(vector<vector<pair<int,int>>> pred, pair<int,int> curr){
    vector<pair<int,int>> ans;
    while(curr.first != -1 && curr.second != -1){
        ans.push_back(curr);
        curr = pred[curr.second][curr.first];
    }
    reverse(ans.begin(), ans.end());
    return ans;
}

void bfs(vector<vector<char>> &grid, pair<int,int> &start, int& treasure, vector<char> ans, vector<vector<bool>> visited, vector<vector<pair<int,int>>> pred, bool tsp = false){
    // get datas
    int n = grid.size(); // row size
    int m = grid[0].size(); // col size
    int x = start.first; // current x coordinate
    int y = start.second; // current y coordinate

    pred[y][x] = {-1,-1}; // set start node's predecessor to -1,-1 since it has no predecessor

    queue<pair<int,int>> q; // queue for bfs
    q.push(start); // push start node into queue
    visited[y][x] = true; // mark start node as visited

    while(!q.empty()){
        pair<int,int> curr = q.front(); // get current node
        q.pop(); // pop current node from queue

        // if treasure found
        if(grid[curr.second][curr.first] == 'T'){
            treasure--; // decrement treasure count
            grid[curr.second][curr.first] = 'R'; // mark treasure as visited
            visited.assign(n, vector<bool>(m, false)); // reset visited since we can revisit nodes
            q.push(curr); // push current node back into queue
            continue;
        }

        // if all treasure found
        if(treasure == 0){
            if(tsp){
                pair<int,int> krusty = findStart(grid);
                grid[krusty.second][krusty.first] = 'T';
                bfs(grid, start, treasure, ans, visited, pred);
            }
            else {
                vector<pair<int,int>> path = getPath(pred, curr);
                possibleBFS.push_back(convertToPath(path));
            }
            return;
        }

        // if not out of bounds, not visited, and not wall
        if(curr.first+1 < m && !visited[curr.second][curr.first+1] && grid[curr.second][curr.first+1] != 'X'){
            q.push({curr.first+1, curr.second}); // push right node into queue
            visited[curr.second][curr.first+1] = true; // mark right node as visited
            pred[curr.second][curr.first+1] = curr; // set right node's predecessor to current node
        }
        if(curr.first-1 >= 0 && !visited[curr.second][curr.first-1] && grid[curr.second][curr.first-1] != 'X'){
            q.push({curr.first-1, curr.second}); // push left node into queue
            visited[curr.second][curr.first-1] = true; // mark left node as visited
            pred[curr.second][curr.first-1] = curr; // set left node's predecessor to current node
        }
        if(curr.second+1 < n && !visited[curr.second+1][curr.first] && grid[curr.second+1][curr.first] != 'X'){
            q.push({curr.first, curr.second+1}); // push down node into queue
            visited[curr.second+1][curr.first] = true; // mark down node as visited
            pred[curr.second+1][curr.first] = curr; // set down node's predecessor to current node
        }
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
            possibleDFS.push_back(ans);
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
    dfs(grid, start, treasure, {}, visited, true);
    for(auto i : possibleDFS){
        for(auto j : i){
            cout << j << " ";
        }
    }
    cout << endl;
    cout << "Nodes: " << dfsCount << endl;
    cout << "Steps: " << possibleDFS[0].size() << endl;
    // cout << "BFS" << endl;
    // vector<vector<pair<int,int>>> pred;
    // pred.assign(grid.size(), vector<pair<int,int>>(grid[0].size(), {-1,-1}));
    // bfs(grid, start, treasure, {}, visited, pred, true);
    // for(auto i : possibleBFS){
    //     for(auto j : i){
    //         cout << j << " ";
    //     }
    // }
    // cout << endl;
    return 0;
}