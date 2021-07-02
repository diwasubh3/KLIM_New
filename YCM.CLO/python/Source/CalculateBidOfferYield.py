import sys 
import math
import json
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import bond_ytm as c

with open(sys.argv[1]) as json_file:  
    data = json.load(json_file)
	
df = pd.DataFrame(data)

df['offerYield'] = df.apply(lambda x: c.bond_ytm(x["OfferPrice"],x["Par"],x["Time"],x["Coupon"],x["Frequncy"],x["Guess"]),axis = 1)
df['bidYield'] = df.apply(lambda x: c.bond_ytm(x["BidPrice"],x["Par"],x["Time"],x["Coupon"],x["Frequncy"],x["Guess"]),axis = 1)

#for row in df.itertuples():
#   print (row[0], row[2])

out = df.to_json(orient='records')

with open(sys.argv[2], 'w') as f:
    f.write(out)
	