#include <bits/stdc++.h>
using namespace std;

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

vector<vector<char>> possibleBFS;
vector<vector<char>> possibleDFS;

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

void bfs(vector<vector<char>> &grid, pair<int,int> &start, int& treasure, vector<char> ans, vector<vector<bool>> visited){
    if(treasure == 0){
        return;
    }
    int n = grid.size();
    int m = grid[0].size();
    int x = start.first;
    int y = start.second;
    const int dx[] = {0,1,0,-1};
    const int dy[] = {1,0,-1,0};
    queue<pair<int,int>> q;
    vector<pair<int,int>> vec;
    q.push({x,y});
    visited[y][x] = true;
    while(!q.empty()){
        // cout<<"here"<<endl;
        int currX = q.front().first;
        int currY = q.front().second;
        q.pop();
        vec.push_back({currX,currY});
        if(grid[currY][currX] == 'T'){
            treasure--;
            grid[currY][currX] = 'R';
            visited.assign(n, vector<bool>(m, false));
            start.first = currX;
            start.second = currY;
            possibleBFS.push_back(convertToPath(vec));
            break;
        }
        for(int i = 0; i < 4; i++){
            int newX = currX + dx[i];
            int newY = currY + dy[i];
            if(newX < 0 || newX >= m || newY < 0 || newY >= n || visited[newY][newX] || grid[newY][newX] == 'X' || (currX = vec.back().first && currY == vec.back().second && (newX == vec[vec.size()-2].first && newY == vec[vec.size()-2].second))){
                continue;
            }
            q.push({newX,newY});
            visited[newY][newX] = true;
        }
    }
}

void dfs(vector<vector<char>> grid, pair<int,int> start, int treasure, vector<char> ans, vector<vector<bool>> visited, bool tsp = false){
    // get datas
    int n = grid.size(); // row size
    int m = grid[0].size(); // col size
    int x = start.first; // current x coordinate
    int y = start.second; // current y coordinate
    if(x < 0 || x >= m || y < 0 || y >= n || visited[y][x] || grid[y][x] == 'X'){ // if out of bounds or visited or wall
        return;
    }

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
        ans.push_back('U');
        dfs(grid, {x,y-1}, treasure, ans, visited, tsp);
        ans.pop_back();
    
        ans.push_back('R');
        dfs(grid, {x+1,y}, treasure, ans, visited, tsp);
        ans.pop_back();
    
        ans.push_back('D');
        dfs(grid, {x,y+1}, treasure, ans, visited, tsp);
        ans.pop_back();
    
        ans.push_back('L');
        dfs(grid, {x-1,y}, treasure, ans, visited, tsp);
        ans.pop_back();
    
    visited[y][x] = false;
}

int main(){
    vector<vector<char>> grid = {{'K','R','R','R'},{'X','R','X','T'},{'X','T','R','R'},{'X','R','X','X'}};
    pair<int,int> start = findStart(grid);
    int treasure = countTreasure(grid);
    vector<vector<bool>> visited(grid.size(), vector<bool>(grid[0].size(), false));
    dfs(grid, start, treasure, {}, visited, true);
    cout << "DFS" << endl;
    cout << "All possible paths are: " << endl;
    for(auto i : possibleDFS){
        for(auto j : i){
            cout << j << " ";
        }
        cout << endl;
    }
    cout << "Optimal path is: " << endl;
    vector<char> ans = findOptimal(possibleDFS);
    for(auto i : ans){
        cout << i << " ";
    }
    cout << endl << endl;
    // cout << "BFS" << endl;
    // while(treasure > 0){
    //     bfs(grid, start, treasure, {}, visited);
    // }
    // bfs(grid, start, treasure, {}, visited);
    // for(auto i : possibleBFS){
    //     for(auto j : i){
    //         cout << j << " ";
    //     }
    // }
    // cout << endl;
    return 0;
}