import numpy as np
import matplotlib.pyplot as plt
import matplotlib.cm as cm
import math

def listPointsTo22DArray(ps):
    return np.array([[e[0], e[1]] for e in ps])

def getSeedCentroids(data, k):
    candidates = []
    for i in range(len(data)):
        candidates.append(i)
    centroids = []
    for iK in range(k):
        rndPI = math.floor(np.random.rand() * len(candidates))
        idx = candidates[rndPI]
        centroids.append([data[idx][0], data[idx][1]])
        candidates.remove(idx)
    return centroids

def getClustersCentroids(clusters):
    centroids = []
    for c in clusters:
        centroids.append(getClusterCentroid(c))
    return centroids

def getClusterCentroid(cluster):
    sumPoint = [0, 0]
    for point in cluster:
        sumPoint[0] += point[0]
        sumPoint[1] += point[1]
    sumPoint[0] /= len(cluster)
    sumPoint[1] /= len(cluster)
    return sumPoint

def getClusters(data, centroids):
    clusters = []
    for i in centroids:
        clusters.append([])

    for dp in data:
        closestClusterIndex = -1
        closestClusterDistance = -1
        for cI in range(len(centroids)):
            cp = centroids[cI]
            currDistance = minkowskiDistance(dp, cp, 2)
            if closestClusterIndex == -1 or currDistance < closestClusterDistance:
                closestClusterDistance = currDistance
                closestClusterIndex = cI
        clusters[closestClusterIndex].append(dp)
    return clusters 

def kMeansClustering(data, k):
    # Step 1: Pick seeds for the k clusters. Maybe consider picking a random ith of the data points
    centroids = getSeedCentroids(data, k)

    # Step 2: Find the distances of each point to the centers and assign each point to a cluster
    clusters = getClusters(data, centroids)

    # Step 3: Find new cluster centroids by find the current clusters center and repeat step 2. Repeat until convergence.
    # Do-While loop
    depth = 0
    while True:
        centroids = getClustersCentroids(clusters)
        newClusters = getClusters(data, centroids)
        depth += 1
        if compareAllClusters(clusters, newClusters) == True or depth == 100000:
            print(clusters)
            print(newClusters)
            print(depth)
            break;

    # Optional step 4: Plot
    # fig, axs = plt.subplots(nrows = 2, ncols = 2)
    # axs[0, 0].set_title('1')
    # showScatterForClusters(clusters, centroids, axs[0, 0])

    # centroids = getClustersCentroids(clusters);
    # clusters = getClusters(data, centroids);
    # axs[0, 1].set_title('2')
    # showScatterForClusters(clusters, centroids, axs[0, 1])

    # centroids = getClustersCentroids(clusters);
    # clusters = getClusters(data, centroids);
    # axs[1, 0].set_title('3')
    # showScatterForClusters(clusters, centroids, axs[1, 0])

    # centroids = getClustersCentroids(clusters);
    # clusters = getClusters(data, centroids);
    # axs[1, 1].set_title('4')
    # showScatterForClusters(clusters, centroids, axs[1, 1])
    # fig.tight_layout()
    showScatterForClusters(clusters, centroids, plt)
    plt.show()

    return "Hello, World!"

def compareAllClusters(clustersA, clustersB):
    for i in range(len(clustersA)):
        if compareClusters(clustersA[i], clustersB[i]) == False:
            return False
    return True

def compareClusters(clustersA, clustersB):
    if len(clustersA) != len(clustersB):
        return False

    for aI in range(len(clustersA)):
        found = False
        for bI in range(len(clustersB)):
            if clustersA[aI] == clustersB[bI]:
                found = True
        if found == False:
            return False
    return True

def showScatterForClusters(clusters, centroids, axes):
    for cs in range(len(clusters)):
        colors = np.array(['b', 'g', 'r','c','m','y','0.2','0.4','0.6','0.8'])
        CLUS = listPointsTo22DArray(clusters[cs])
        map = np.array([cs % len(colors)] * len(CLUS))
        axes.scatter(CLUS[ : , 0], CLUS[ :, 1], s = 5, c = colors[map], marker = 's')
    CENS = listPointsTo22DArray(centroids)
    axes.scatter(CENS[ : , 0], CENS[ :, 1], s = 100, c = 'k', marker = 'H')

def random_color():
    return list(np.random.choice(range(256), size = 3))

def minkowskiDistance(vecA, vecB, r):
    sum = 0
    for comp in range(len(vecA)):
        sum += (abs(vecA[comp] - vecB[comp]))**r
    return sum**(1 / r)

dat = []
for i in range(50):
    dat.append([np.random.rand(), np.random.rand()])
kMeansClustering(dat, 2)