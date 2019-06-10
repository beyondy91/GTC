- input
    + json
    
    + { "samples": ["string"], 
    
        "snps": ["string"],
        
        "token": int (optional)}
    
- output
    + json
    
    + { "meta" : {snp_name: [allele_1, allele_2]},
    
    "genotype": [genotype_sample1, genotype_sample2, ...] }
    
    
- limit
    + max_page = 10000
    + chunk_size = len(samples)
    + snp_size = round(max_page / chunk_size)
    + page_size = chunk_size * snp_size